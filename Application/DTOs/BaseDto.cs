using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record BaseDto(Guid Id, [DataType(DataType.Date)] DateTime CreatedDate, [DataType(DataType.Date)] DateTime UpdatedDate, string UpdatedBy, string AddedBy, bool IsDeleted);
