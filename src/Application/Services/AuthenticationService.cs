﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
    Task RevokeTokens(string username);
    Task<string> GenerateRefreshJwtToken(User user);
    Task<string> GenerateAccessJwtToken(User user);
    DateTime GetRefreshTokenExpiration();
    DateTime GetAccessTokenExpiration();
    Task<ClaimsPrincipal?> ValidateToken(string token);
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
        var revokedToken = await _repository.JwtTokens!
                .FindByCondition(t => t.TokenValue == token && !t.IsRevoked)
                .FirstOrDefaultAsync();
        return revokedToken == null;
    }

    public async Task<ClaimsPrincipal?> ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtBearer:Key"]!);

        var validationParameters = new TokenValidationParameters
        {
            ValidIssuer = _configuration["JwtBearer:Issuer"],
            ValidAudience = _configuration["JwtBearer:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
        };

        try
        {
            var principal = await Task.Run(() => tokenHandler.ValidateToken(token, validationParameters, out var validatedToken));
            return principal;
        }
        catch
        {
            return null;
        }
    }

    public async Task RevokeTokens(string username)
    {
        var tokens = await _repository.JwtTokens!.FindByCondition(t => t.UserName == username).ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevocationDate = DateTime.UtcNow;
            _repository.JwtTokens.Update(token);
        }

        await _repository.SaveAsync();
    }

    public async Task<string> GenerateRefreshJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["JwtBearer:Key"]!);

        var tokenOptions = GenerateTokenOptions(
            new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            await GetClaims(user, _configuration["JwtBearer:Refresher"]!),
            GetRefreshTokenExpiration());

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        _repository.JwtTokens!.Add(new JwtTokens()
        {
            UserName = user.UserName!,
            TokenValue = token,
            ExpirationDate = GetRefreshTokenExpiration()
        });
        await _repository.SaveAsync();

        return token;
    }

    public async Task<string> GenerateAccessJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["JwtBearer:Key"]!);

        var tokenOptions = GenerateTokenOptions(
            new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            await GetClaims(user, _configuration["JwtBearer:Accessor"]!),
            GetAccessTokenExpiration());

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        _repository.JwtTokens!.Add(new JwtTokens()
        {
            UserName = user.UserName!,
            TokenValue = token,
            ExpirationDate = GetAccessTokenExpiration()
        });
        await _repository.SaveAsync();

        return token;
    }

    private async Task<List<Claim>> GetClaims(User user, string tokenType)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("tokenType", tokenType)
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

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims, DateTime expiration)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["JwtBearer:Issuer"],
            audience: _configuration["JwtBearer:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

    public DateTime GetRefreshTokenExpiration() => DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JwtBearer:RefreshTokenExpiration"]));

    public DateTime GetAccessTokenExpiration() => DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JwtBearer:AccessTokenExpiration"]));
}
