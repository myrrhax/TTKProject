﻿namespace ImageService.Contracts;

public class ImageResponse
{
    public Guid Id { get; set; }
    public string FilePath { get; set; } = null!;
}
