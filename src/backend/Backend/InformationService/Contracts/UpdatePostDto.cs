﻿namespace InformationService.Contracts;

public class UpdatePostDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid? ImageId { get; set; } 
}
