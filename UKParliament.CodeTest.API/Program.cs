using UKParliament.CodeTest.API;
using UKParliament.CodeTest.API.Helper;
using UKParliament.CodeTest.API.Middleware;
using UKParliament.CodeTest.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("https://localhost:44416") // Angular's origin
              .AllowAnyMethod() // Allow all HTTP methods (GET, POST, PUT, DELETE)
              .AllowAnyHeader() // Allow all headers
              .AllowCredentials(); // Allow cookies or credentials if needed
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ValidateModelFilter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureServices();

var app = builder.Build();

// Enable CORS
app.UseCors("AllowAngularApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
    {
        using var context = serviceScope.ServiceProvider.GetRequiredService<PersonManagerContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ResponseWrapperMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
