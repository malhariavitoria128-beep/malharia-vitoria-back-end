using System.Security.Claims;

namespace malharia_back_end.Middleware
{
	public class SimpleRequestLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly string _logFilePath = @"C:\LogsMeuApp\requests-log.txt";

		public SimpleRequestLoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			// Garante que a pasta exista
			var logDir = Path.GetDirectoryName(_logFilePath);
			if (!Directory.Exists(logDir))
			{
				Directory.CreateDirectory(logDir);
			}

			// Pega usuário logado
			var userEmail = context.User?.FindFirst(ClaimTypes.Email)?.Value ?? "Não logado";
			var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "N/A";

			// Pega rota e método
			var method = context.Request.Method;
			var path = context.Request.Path;

			// Linha de log
			var logLine = $"Horário: {DateTime.Now:yyyy-MM-dd HH:mm:ss} | Usuário: {userEmail} ({userId}) | Método: {method} | Endpoint: {path}";

			// Grava em arquivo
			await File.AppendAllTextAsync(_logFilePath, logLine + Environment.NewLine);

			// Continua pipeline
			await _next(context);
		}
	}

}
