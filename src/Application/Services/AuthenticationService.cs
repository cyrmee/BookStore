﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public interface IAuthenticationService
{
    Task<bool> IsTokenRevoked(string token);
    Task<string?> RevokeToken(string token);
    Task<string> GenerateJwtToken(User user);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IRepository _repository;

    public AuthenticationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IRepository repository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _repository = repository;
    }

    public async Task<bool> IsTokenRevoked(string token)
    {
        var revokedToken = await _repository.JwtTokens!.FindByCondition(t => t.TokenValue == token).FirstOrDefaultAsync();
        return revokedToken != null || revokedToken!.IsRevoked;
    }

    public async Task<string?> RevokeToken(string token)
    {
        var revokedToken = await _repository.JwtTokens!.FindByCondition(t => t.TokenValue == token).FirstOrDefaultAsync();
        revokedToken!.IsRevoked = true;
        _repository.JwtTokens.Update(revokedToken);
        await _repository.SaveAsync();
        return revokedToken?.TokenValue;
    }

    public async Task<string> GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["JwtBearer:Key"]!);

        var tokenOptions = GenerateTokenOptions(
            new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            await GetClaims(user));

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        _repository.JwtTokens!.Add(entity: new JwtTokens()
        {
            UserName = user.UserName!,
            TokenValue = token,
            ExpirationDate = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JwtBearer:TokenExpiration"]))
        });
        await _repository.SaveAsync();

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(
            roles.Select(role => new Claim(ClaimTypes.Role, role)
            ));

        foreach (var r in roles)
        {
            var role = await _roleManager.Roles
                .Where(m => m.Name!.ToUpper() == r.ToUpper())
                .FirstOrDefaultAsync();

            if (role != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }
        }

        claims.Add(new Claim(ClaimTypes.UserData, JsonSerializer.Serialize(user, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve
        })));
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            audience: _configuration["JwtBearer:Audience"],
            issuer: _configuration["JwtBearer:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JwtBearer:TokenExpiration"])),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
}