var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(
    setupAction =>
    {
        setupAction.AddDefaultPolicy(
            policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
    });

var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.MapReverseProxy();

app.Run();
