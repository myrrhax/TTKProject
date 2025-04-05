namespace InformationService.Contracts;

public record PostDto(Guid PostId, 
    string Title, 
    string Content, 
    DateTime CreationDate, 
    Guid? ImageId, 
    Guid CreatorId,
    IEnumerable<HistoryDto> History
);
