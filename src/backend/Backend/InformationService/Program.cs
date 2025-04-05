using Carter;
using InformationService.Configuration;
using InformationService.DataAccess;
using InformationService.Entities;
using InformationService.Interactors;
using InformationService.Interactors.CreatePost;
using InformationService.Interactors.DeletePost;
using InformationService.Interactors.GetHistory;
using InformationService.Interactors.GetPost;
using InformationService.Interactors.GetPosts;
using InformationService.Interactors.RestorePost;
using InformationService.Interactors.UpdatePost;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddDocumentation();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCarter();

#region Interactors
builder.Services.AddScoped<IBaseInteractor<CreatePostParams, Post>, CreatePostInteractor>();
builder.Services.AddScoped<IBaseInteractor<DeletePostParams, Guid>, DeletePostInteractor>();
builder.Services.AddScoped<IBaseInteractor<Guid, Post>, GetPostInteractor>();
builder.Services.AddScoped<IBaseInteractor<GetPostsParams, IEnumerable<Post>>, GetPostsInteractor>();
builder.Services.AddScoped<IBaseInteractor<UpdatePostParams, Guid>, UpdatePostInteractor>();
builder.Services.AddScoped<IBaseInteractor<RestorePostParams, bool>, RestorePostInteractor>();
builder.Services.AddScoped<IBaseInteractor<GetHistoryParams, IEnumerable<PostHistory>>, GetHistoryInteractor>();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(opt =>
    {
        opt.RouteTemplate = "openapi/{documentName}.json";
    });

    app.MapScalarApiReference(opt =>
    {
        opt.Title = "WebChatAPI";
        opt.Theme = ScalarTheme.Mars;
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapCarter();
app.Run();