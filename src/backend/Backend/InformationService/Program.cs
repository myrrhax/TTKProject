using Carter;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using InformationService.Background;
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

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "information", // Название должно совпадать с SwaggerKey в ocelot.json
        Version = "v1"
    });
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

#region Interactors
builder.Services.AddScoped<IBaseInteractor<CreatePostParams, Post>, CreatePostInteractor>();
builder.Services.AddScoped<IBaseInteractor<DeletePostParams, Guid>, DeletePostInteractor>();
builder.Services.AddScoped<IBaseInteractor<Guid, Post>, GetPostInteractor>();
builder.Services.AddScoped<IBaseInteractor<GetPostsParams, GetPostsResult>, GetPostsInteractor>();
builder.Services.AddScoped<IBaseInteractor<UpdatePostParams, Guid>, UpdatePostInteractor>();
builder.Services.AddScoped<IBaseInteractor<GetHistoryParams, IEnumerable<PostHistory>>, GetHistoryInteractor>();
builder.Services.AddScoped<IBaseInteractor<RestorePostParams, bool>, RestorePostInteractor>();
#endregion

builder.Services.AddCarter();
builder.Services.AddHostedService<DeleteOldPosts>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "information");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapCarter();
app.Run();
