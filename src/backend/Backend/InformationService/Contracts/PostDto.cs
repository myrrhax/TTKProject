namespace InformationService.Contracts;

public record PostDto(string PostId, 
    string Title, 
    string Content, 
    DateTime CreationDate, 
    Guid? ImageId, 
    Guid CreatorId,
    IEnumerable<HistoryDto> History
);
