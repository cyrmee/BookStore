using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class JwtTokens : BaseEntity
{
    public required string UserName { get; set; }
    public required string TokenValue { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    public DateTime ExpirationDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    public DateTime RevocationDate { get; set; }
    public bool IsRevoked { get; set; } = false;

    public User User { get; set; } = null!;
}
