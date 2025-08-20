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
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddLocalhostCors();

var app = builder.Build();
DbInitializer.Initialize(app.Services);

// Swagger apenas no ambiente de Dev
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Redireciona para HTTPS se configurado (opcional para Cloudflare)
app.UseHttpsRedirection();

// CORS deve vir cedo, antes dos controllers
app.UseCors("LiberarLocalhost4200");

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Middleware de log (opcional, pode ativar se quiser ver os requests)
app.UseMiddleware<SimpleRequestLoggingMiddleware>();

// Serve SPA (Angular)
app.UseDefaultFiles();   // procura index.html
app.UseStaticFiles();    // JS, CSS, etc

// API controllers
app.MapControllers();

// Fallback para Angular SPA (evita erro 404 ao recarregar páginas)
app.MapFallbackToFile("index.html");

app.Run();
