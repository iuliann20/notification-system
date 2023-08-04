using Hangfire;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NotificationSystem.Common;
using NotificationSystem.Common.Implementation;
using NotificationSystem.Common.Interfaces;
using NotificationSystem.Common.Settings;
using NotificationSystem.DataAccessLayer;
using NotificationSystem.Hangfire.HangfireAuthorization;
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
                        Title = "NotificationSystem APi",
                        Description = "API used for the NotificationSystem application.",
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

    var encryption = new EncryptionService();
    var connString = encryption.Decrypt(builder.Configuration["AppSettings:DatabaseConnection"]);

    builder.Services.AddHangfire(conf=> conf.UseSqlServerStorage(connString));
    builder.Services.AddHangfireServer();

    DataConfiguration.RegisterDependencies(builder.Services);
    CommonConfiguration.RegisterDependencies(builder.Services);
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
    app.UseAuthentication();
    app.UseHttpsRedirection();

    var appSettings = app.Services.GetService<IOptions<AppSettings>>().Value;
    var encryptionService = app.Services.GetService<IEncryptionService>();

    app.UseHangfireServer();
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
        {
            RequireSsl = false,
            SslRedirect = false,
            LoginCaseSensitive = true,
            Users = new []
            {
                new BasicAuthAuthorizationUser
                {
                    Login = encryptionService.Decrypt(appSettings.HangfireUserLogin),
                    PasswordClear = encryptionService.Decrypt(appSettings.HangfireUserPassword)
                }
            }
        })}
    });
    var recurringJobOptions = new RecurringJobOptions { TimeZone = TimeZoneInfo.Local };
    //RecurringJob.AddOrUpdate<EnrollmentRequestWorker>("EmailsJob", job => job.RunEmailJob(), appSettings.JobsCron.EmailCron, recurringJobOptions);

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