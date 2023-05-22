using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record BaseDto(Guid Id, [DataType(DataType.Date)] DateTime CreatedDate, [DataType(DataType.Date)] DateTime UpdatedDate, string UpdatedBy, string AddedBy, bool IsDeleted)
{
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    public DateTime CreatedDate { get; set; } = CreatedDate;

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    public DateTime UpdatedDate { get; set; } = UpdatedDate;
    public string UpdatedBy { get; set; } = UpdatedBy;
    public string AddedBy { get; set; } = AddedBy;
    public bool IsDeleted { get; set; } = IsDeleted;
}
