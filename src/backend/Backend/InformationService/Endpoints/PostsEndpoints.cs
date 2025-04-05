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
