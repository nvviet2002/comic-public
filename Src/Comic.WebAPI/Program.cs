using Comic.Infrastructure.Extensions;
using Comic.WebAPI.Common;
using Comic.WebAPI.Middlewares;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Text.Json;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(options =>
{
    options.AddDebug();
    options.AddConsole();
    //etc
});
// Add services to the container.
builder.Services.AddConnections();
builder.Services.LoadAppSetting(builder.Configuration);
builder.Services.AddComicDbContext();
builder.Services.AddComicIdentity();
builder.Services.AddUnitOfWork();
builder.Services.AddAppServices();
builder.Services.AddAppAuthetication();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = (actionContext) =>
    {
        return ModelStateHelper.ModelStateResponse(actionContext);
    };
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
})
;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

//// File upload max size, these settings will take effect on all APIs
//builder.Services.Configure<KestrelServerOptions>(options =>
//{
//    options.Limits.MaxRequestBodySize = 1_000_000; // if don't set default value is: 30 MB
//});
//builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(x =>
//{
//    x.ValueLengthLimit = 1_000_000;
//    x.MultipartBodyLengthLimit = 1_000_000; // if don't set default value is: 128 MB
//    x.MultipartHeadersLengthLimit = 1_000_000;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseMiddleware<ExceptionMiddleware>();

app.Run();
