namespace InformationService.Contracts;

public record HistoryDto(Guid PostId, 
    string Title, 
    Guid RedactorId, 
    string EditType, 
    DateTime UpdateTime
);