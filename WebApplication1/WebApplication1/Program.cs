var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // For development, allow any origin (adjust this later for production)
        policy.WithOrigins("https://taupe-malabi-0079ea.netlify.app")

              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("Content-Type");
    });
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply CORS policy globally (must be done before routing and authorization)
app.UseCors("AllowFrontend");

app.UseRouting();

// Swagger configuration for development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
