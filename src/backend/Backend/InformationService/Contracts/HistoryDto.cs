namespace InformationService.Contracts;

public record HistoryDto(string PostId, 
    string Title, 
    Guid RedactorId, 
    string EditType, 
    DateTime UpdateTime
);