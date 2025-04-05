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
        group.MapPut("/{id:guid}", UpdatePost)
            .WithOpenApi();
        group.MapDelete("/{id:guid}", DeletePost)
            .WithOpenApi();
        group.MapPost("/restore", RestorePost)
            .WithOpenApi();
    }

    public async Task<Results<Ok<PostDto>, NotFound>> GetPostById(Guid id, IBaseInteractor<Guid, Post> interactor)
    {
        var postResult = await interactor.ExecuteAsync(id);

        if (postResult.IsSuccess)
        {
            return TypedResults.Ok<PostDto>(postResult.Value);
        }

        return TypedResults.NotFound();
    }

    public async Task<Results<Ok<List<PostDto>>, NotFound>> GetPosts(IBaseInteractor<GetPostsParams, IEnumerable<Post>> interactor,
        [FromQuery] int page = 1, 
        [FromQuery] string query = "", 
        [FromQuery] string orderBy = "desc")
    {
        var sortType = orderBy == "asc" ? DateSortType.Ascending : DateSortType.Descending;
        
        var param = new GetPostsParams(page, query, sortType);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            var dtos = new List<PostDto>();
            foreach (var post in result.Value)
            {
                dtos.Add(post);
            }

            return TypedResults.Ok(dtos);
        }

        return TypedResults.NotFound();
    }

    public async Task<Results<Ok, NotFound>> DeletePost(Guid id, IBaseInteractor<DeletePostParams, Guid> interactor)
    {
        var userId = Guid.NewGuid(); // ToDo change to real user
        var result = await interactor.ExecuteAsync(new DeletePostParams(id, userId));

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }
        return TypedResults.NotFound();
    }

    public async Task<Results<Ok, NotFound>> RestorePost([FromBody] RestorePostDto dto, IBaseInteractor<RestorePostParams, bool> interactor)
    {
        if (!Guid.TryParse(dto.PostId, out Guid id))
        {
            return TypedResults.NotFound();
        }
        var userId = Guid.NewGuid(); // ToDo change to real user
        var result = await interactor.ExecuteAsync(new RestorePostParams(id, userId));

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }
        return TypedResults.NotFound();
    }

    public async Task<Results<Ok<Guid>, BadRequest<ErrorsContainer>>> UpdatePost(Guid id,
        [FromBody] UpdatePostDto dto,
        IBaseInteractor<UpdatePostParams, Guid> interactor)
    {
        
        Guid userId = Guid.NewGuid(); // ToDo change to real user
        var param = new UpdatePostParams(id, userId, dto.Title, dto.Content, dto.ImageId);

        var result = await interactor.ExecuteAsync(param);
        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        return TypedResults.BadRequest(result.Error);
    }

    public async Task<Results<Created, BadRequest<ErrorsContainer>>> AddPost(IBaseInteractor<CreatePostParams, Post> interactor,
        [FromBody] CreatePostDto dto)
    {
        Guid userGuid = Guid.NewGuid(); // ToDo change to real user
        var param = new CreatePostParams(dto.Title, userGuid, dto.Content, dto.ImageId);
        var result = await interactor.ExecuteAsync(param);

        if (result.IsSuccess)
        {
            return TypedResults.Created();
        }

        return TypedResults.BadRequest(result.Error);
    }
}
