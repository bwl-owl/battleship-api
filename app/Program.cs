using FluentValidation;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.EnableAnnotations());

builder.Services.AddMvc().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters = new List<JsonConverter>() { new StringEnumConverter() };
});

builder.Services
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
