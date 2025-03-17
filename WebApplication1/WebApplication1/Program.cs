var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        // For development, allow any origin (you can restrict this later for production)
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin();  // Allow all origins during development
        }
        else
        {
            // In production, restrict to a specific origin (replace with your frontend domain)
            policy.WithOrigins("https://sportcityapi.azurewebsites.net/")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

// Add controllers and other services
builder.Services.AddControllers();

// Add other services (Swagger, etc.)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use CORS policy before routing and authorization middleware
app.UseCors("AllowSpecificOrigin");

app.UseRouting();

// Swagger middleware for development
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
