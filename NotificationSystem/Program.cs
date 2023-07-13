using Microsoft.OpenApi.Models;
using NotificationSystem.Common;
using NotificationSystem.DataAccessLayer;
using Serilog;

Log.Logger = new LoggerConfiguration()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "CinemaOnline API",
                        Description = "API used for the CinemaOnline application.",
                        Contact = new OpenApiContact
                        {
                            Name = "Silitra Iulian-Alexandru",
                            Email = "silitra.iulian1997@gmail.com"
                        }
                    });

        c.AddSecurityDefinition("Bearer",
            new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
                });
        c.UseInlineDefinitionsForEnums();
    });

    DataConfiguration.RegisterDependencies(builder.Services);
    CommonConfiguration.RegisterDependencies(builder.Services);
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex, "An error occurred.");
}
finally
{
    Log.CloseAndFlush();
}