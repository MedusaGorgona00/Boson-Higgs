using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using BosonHiggsApi.BL.Extensions;
using BosonHiggsApi.BL.Interfaces;
using BosonHiggsApi.BL.Models.Options;
using BosonHiggsApi.BL.Services;
using BosonHiggsApi.BL.Services.Email;
using BosonHiggsApi.DL;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace BosonHiggsApi.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<AccountService>();
            services.AddScoped<LevelService>();
            services.AddScoped<LeaderBoardService>();
            services.AddScoped<BruteforceService>();
            services.AddScoped<ChatService>();

            return services;;
        }

        public static IServiceCollection AddDbCtx(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default")));
            return services;
        }

        public static IServiceCollection AddSerilog1(this IServiceCollection services, IConfiguration configuration, ILoggingBuilder logging)
        {
            var connectionString = configuration.GetConnectionString("Default");
            AutoCreateLogDatabase(connectionString);

            var columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {"Message", new RenderedMessageColumnWriter() },
                {"Level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {"CreatedTimeUtc", new TimestampColumnWriter() }
            };

            logging.AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware", LogEventLevel.Information)
                .WriteTo.PostgreSQL(connectionString.ToLoggingDbConnStr(), "Logs", columnWriters, needAutoCreateTable: true)
                .CreateLogger());

            return services;
        }

        private static void AutoCreateLogDatabase(string connectionString)
        {
            var connection = new NpgsqlConnection(connectionString);
            try
            {
                var regex = new Regex("Database=(.*?);");
                var databaseName = regex.Match(connectionString).Groups[1].Value + "_log";
                var command = new NpgsqlCommand("CREATE DATABASE " + databaseName, connection);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
        }

        public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection("EmailOptions").Get<EmailOptions>();
            EmailOptions.Validate(options);

            services.AddScoped<IEmailSender, EmailSender>()
                .AddScoped<AggregateEmailSender>()
                .AddFluentEmail(defaultFromEmail: options.FromEmail, defaultFromName: options.FromName);

            services.AddScoped<FluentEmail.Core.Interfaces.ISender>(provider =>
            {
                return new FluentEmail.Smtp.SmtpSender(new Func<SmtpClient>(() =>
                {
                    var client = new SmtpClient(options.SMTP.Host, options.SMTP.Port!.Value)
                    {
                        EnableSsl = true,
                        Credentials = new NetworkCredential(options.FromEmail, options.SMTP.Password)
                    };

                    client.SendCompleted += (sender, e) =>
                    {
                        if (e.Error != null)
                            System.Diagnostics.Trace.TraceError($"Error sending email: {e.Error.Message}");
                        if (sender is SmtpClient s)
                            s.Dispose();
                    };
                    return client;
                }));
            });

            return services;
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));

            return services;
        }
        
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, string corsPolicyName,
            IConfiguration configuration)
        {
            var corsOption = configuration.GetSection("CORS").Get<CorsOptions>() ?? new CorsOptions();

            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName,
                    builder =>
                    {
                        if (corsOption.AllowedHosts != null && corsOption.AllowedHosts.Any())
                        {
                            builder.WithOrigins(corsOption.AllowedHosts.ToArray());
                        }
                        else
                        {
                            builder.AllowAnyOrigin();
                        }

                        if (corsOption.AllowedHeaders != null && corsOption.AllowedHeaders.Any())
                        {
                            builder
                                .WithHeaders(corsOption.AllowedHeaders.ToArray());
                        }
                        else
                        {
                            builder.AllowAnyHeader();
                        }

                        builder
                            .AllowAnyMethod();
                    }
                );
            });

            return services;
        }
    }
}
