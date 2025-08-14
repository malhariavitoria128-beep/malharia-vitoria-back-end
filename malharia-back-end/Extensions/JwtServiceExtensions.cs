using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

public static class JwtServiceExtensions
{
	public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
	{
		var jwt = configuration.GetSection("Jwt");
		var key = Encoding.UTF8.GetBytes(jwt["Key"]);

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.RequireHttpsMetadata = false;
			options.SaveToken = true;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = jwt["Issuer"],
				ValidateAudience = true,
				ValidAudience = jwt["Audience"],
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};

			options.Events = new JwtBearerEvents
			{
				OnAuthenticationFailed = ctx =>
				{
					if (ctx.Exception is SecurityTokenExpiredException)
						ctx.Response.Headers.Append("Token-Expired", "true");
					return Task.CompletedTask;
				},
				OnChallenge = ctx =>
				{
					ctx.HandleResponse();
					if (!ctx.Response.HasStarted)
					{
						ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
						ctx.Response.ContentType = "application/json";
						ctx.Response.Headers.Append(
							"WWW-Authenticate",
							"Bearer error=\"invalid_token\", error_description=\"The access token is invalid or has expired\""
						);

						var payload = JsonSerializer.Serialize(new
						{
							error = "unauthorized",
							message = "Token inválido ou expirado."
						});
						return ctx.Response.WriteAsync(payload);
					}
					return Task.CompletedTask;
				},
				OnForbidden = ctx =>
				{
					ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
					ctx.Response.ContentType = "application/json";
					var payload = JsonSerializer.Serialize(new
					{
						error = "forbidden",
						message = "Você não tem permissão para acessar este recurso."
					});
					return ctx.Response.WriteAsync(payload);
				}
			};
		});

		return services;
	}
}
