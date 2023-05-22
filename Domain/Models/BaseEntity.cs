using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class BaseEntity
{
    public Guid Id { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    public DateTime CreatedDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    public DateTime UpdatedDate { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public string AddedBy { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
}
