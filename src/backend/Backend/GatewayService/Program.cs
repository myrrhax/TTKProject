using MMLib.SwaggerForOcelot.DependencyInjection;
using MMLib.SwaggerForOcelot.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ������������ ocelot.json
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// ���������� Ocelot � SwaggerForOcelot
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy =>
{
    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
}));


var app = builder.Build();

app.MapOpenApi();

app.UseAuthorization();

app.UseCors("AllowAll");
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs"; // ���� ���� �����!
});

await app.UseOcelot();

app.Run();
