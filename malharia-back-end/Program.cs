using malharia_back_end.Middleware;
using malharia_back_end.Services.Interfaces;
using malharia_back_end.Services.Services;
using malharia_back_end.Static;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddLocalhostCors();
var app = builder.Build();
DbInitializer.Initialize(app.Services);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors("LiberarLocalhost4200");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<SimpleRequestLoggingMiddleware>();

app.MapControllers();

app.Run();
