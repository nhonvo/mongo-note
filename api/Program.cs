using System.Collections.Immutable;
using api;
using api.Model;
using api.Service;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var configure = builder.Configuration;
var configuration = builder.Configuration.Get<AppSettings>() ?? throw new Exception();

builder.Services.AddSingleton(configuration);

// Add services to the container.
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddSingleton<BooksService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();