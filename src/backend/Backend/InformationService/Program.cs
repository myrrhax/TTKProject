using Carter;
using InformationService.Background;
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
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapCarter();
app.Run();