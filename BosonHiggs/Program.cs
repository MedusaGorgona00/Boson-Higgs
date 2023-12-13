using BosonHiggsApi.BL;
using BosonHiggsApi.DL;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var logging = builder.Logging;

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbCtx(configuration)
    //.AddSerilog1(configuration, logging) TODO: recomment after changing connectionString
    .RegisterServices()
    .AddEmailServices(configuration)
    .AddOptions(configuration);

var app = builder.Build();

await Seed.SeedDatabaseAsync(app.Services);

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//if (app.Environment.IsDevelopment()) //TODO: change 16
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Boson-Higgs API v1");
        c.DocumentTitle = "Online Boson-Higgs service API Documentation";

        c.DefaultModelRendering(ModelRendering.Example);
        c.DefaultModelsExpandDepth(-1);
        c.DisplayOperationId();
        c.DocExpansion(DocExpansion.None);
        c.EnableDeepLinking();
        c.EnableFilter();
        c.ShowExtensions();
    });
//}
app.MapHub<ChatHub>("/chat");
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
