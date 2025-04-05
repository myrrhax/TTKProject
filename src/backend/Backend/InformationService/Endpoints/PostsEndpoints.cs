using Carter;
using InformationService.Contracts;
using InformationService.Entities;
using InformationService.Interactors;
using InformationService.Interactors.CreatePost;
using InformationService.Interactors.GetPosts;
using InformationService.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InformationService.Endpoints;

public class PostsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/posts");

        group.MapGet("/{id:guid}", GetPostById)
            .WithOpenApi();

        group.MapGet("", GetPosts)
            .WithOpenApi();

        group.MapPost("", AddPost)
            .WithOpenApi();
    }

    public async Task<Results<Ok<PostDto>, NotFound>> GetPostById(
        Guid id,
        IBaseInteractor<Guid, Post> interactor)
    {
        var result = await interactor.ExecuteAsync(id);

        if (result.IsSuccess)
        {
            var dto = MapToDto(result.Value);
            return TypedResults.Ok(dto);
        }

        return TypedResults.NotFound();
    }

    public async Task<Results<Ok<GetPostsDto>, NotFound>> GetPosts(
        IBaseInteractor<GetPostsParams, GetPostsResult> interactor,
        [FromQuery] int page = 1,
        [FromQuery] string query = "",
        [FromQuery] string orderBy = "desc")
    {
        var sortType = orderBy == "asc" ? DateSortType.Ascending : DateSortType.Descending;
        var param = new GetPostsParams(page, query, sortType);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            var dto = new GetPostsDto
            {
                MaxPages = result.Value.MaxPages,
                Posts = result.Value.Posts.Select(MapToDto).ToList()
            };

            return TypedResults.Ok(dto);
        }

        return TypedResults.NotFound();
    }

    public async Task<Results<Created, BadRequest<ErrorsContainer>>> AddPost(
        IBaseInteractor<CreatePostParams, Post> interactor,
        [FromBody] CreatePostDto dto)
    {
        Guid userGuid = Guid.NewGuid(); // TODO: Заменить на реального пользователя
        var param = new CreatePostParams(dto.Title, userGuid, dto.Content, dto.ImageId);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            return TypedResults.Created();
        }

        return TypedResults.BadRequest(result.Error);
    }

    private PostDto MapToDto(Post post)
    {
        return new PostDto
        {
            PostId = post.PostId,
            Title = post.Title,
            Content = post.Content,
            CreationTime = post.CreationTime,
            CreatorId = post.CreatorId,
            ImageId = post.ImageId,
            History = post.History.Select(h => (HistoryDto)h).ToList()
        };
    }
}

public class GetPostsDto
{
    public List<PostDto> Posts { get; set; } = new();
    public int MaxPages { get; set; }
}
