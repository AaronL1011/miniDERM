using System.Net.WebSockets;
using Orleans.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:8080");

// Add services to the container.
builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering()
    .Configure<ClusterOptions>(options => { 
        options.ClusterId = "dev";
        options.ServiceId = "DERMS";
    })
    .AddMemoryGrainStorage("MemoryStore");
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.UseWebSockets();

app.MapControllers();

app.Run();
