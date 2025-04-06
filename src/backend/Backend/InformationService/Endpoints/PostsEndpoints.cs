using Carter;
using InformationService.Contracts;
using InformationService.Entities;
using InformationService.Interactors;
using InformationService.Interactors.CreatePost;
using InformationService.Interactors.DeletePost;
using InformationService.Interactors.GetPosts;
using InformationService.Interactors.RestorePost;
using InformationService.Interactors.UpdatePost;
using InformationService.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InformationService.Endpoints;

public class PostsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/posts").RequireAuthorization();

        group.MapGet("/{id:guid}", GetPostById).WithOpenApi();
        group.MapGet("", GetPosts).WithOpenApi();
        group.MapPost("", AddPost).WithOpenApi();
        group.MapPut("/{id:guid}", UpdatePost).WithOpenApi();
        group.MapDelete("/{id:guid}", DeletePost).WithOpenApi();
        group.MapPost("/restore", RestorePost).WithOpenApi();
    }

    private async Task<Results<Ok, BadRequest<ErrorsContainer>>> UpdatePost(IBaseInteractor<UpdatePostParams, Guid> interactor,
        Guid id, 
        [FromBody]UpdatePostDto dto,
        ClaimsPrincipal claims)
    {
        var userId = GetUserId(claims);
        var param = new UpdatePostParams(id, userId, dto.Title, dto.Content, dto.ImageId);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        return TypedResults.BadRequest(result.Error);
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

    public async Task<Results<Ok, NotFound>> DeletePost(Guid id, IBaseInteractor<DeletePostParams, Guid> interactor, ClaimsPrincipal claims)
    {
        var userId = GetUserId(claims);
        var result = await interactor.ExecuteAsync(new DeletePostParams(id, userId));

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }
        return TypedResults.NotFound();
    }

    public async Task<Results<Ok, NotFound>> RestorePost([FromBody] RestorePostDto dto, IBaseInteractor<RestorePostParams, bool> interactor, ClaimsPrincipal claims)
    {
        if (!Guid.TryParse(dto.PostId, out Guid id))
        {
            return TypedResults.NotFound();
        }
        var userId = GetUserId(claims);
        var result = await interactor.ExecuteAsync(new RestorePostParams(id, userId));

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }
        return TypedResults.NotFound();
    }

    public async Task<Results<Created, BadRequest<ErrorsContainer>>> AddPost(
        IBaseInteractor<CreatePostParams, Post> interactor,
        [FromBody] CreatePostDto dto,
        ClaimsPrincipal claims)
    {
        var userId = GetUserId(claims);
        var param = new CreatePostParams(dto.Title, userId, dto.Content, dto.ImageId);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            return TypedResults.Created();
        }

        return TypedResults.BadRequest(result.Error);
    }

    private Guid GetUserId(ClaimsPrincipal claims)
    {
        return Guid.Parse(claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
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
