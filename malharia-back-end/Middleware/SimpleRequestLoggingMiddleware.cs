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
			var logDir = Path.GetDirectoryName(_logFilePath);
			if (!Directory.Exists(logDir))
			{
				Directory.CreateDirectory(logDir);
			}

			var userEmail = context.User?.FindFirst(ClaimTypes.Email)?.Value ?? "Não logado";
			var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "N/A";

			var method = context.Request.Method;
			var path = context.Request.Path;

			var logLine = $"Horário: {DateTime.Now:yyyy-MM-dd HH:mm:ss} | Usuário: {userEmail} ({userId}) | Método: {method} | Endpoint: {path}";

			await File.AppendAllTextAsync(_logFilePath, logLine + Environment.NewLine);

			await _next(context);
		}
	}

}
