using malharia_back_end.Middleware;
using malharia_back_end.Services.Interfaces;
using malharia_back_end.Services.Services;
using malharia_back_end.Static;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddJwtAuth(builder.Configuration);

// CORS já está implementado na extensão AllowFirebase
builder.Services.AllowFirebase();

var app = builder.Build();

// Inicializa DB
DbInitializer.Initialize(app.Services);

// Middleware customizado
app.UseMiddleware<SimpleRequestLoggingMiddleware>();

// Swagger em desenvolvimento
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS antes da auth
app.UseCors("AllowFirebase");

app.UseAuthentication();
app.UseAuthorization();

// Map controllers SEM mexer no padrão (mas garantindo que a rota tenha "api/...")
app.MapControllers();

// Servir Angular do wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

// Fallback para Angular SPA
app.MapFallbackToFile("index.html");

app.Run();

