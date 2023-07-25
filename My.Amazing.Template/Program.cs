using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using My.Amazing.Template.Data;

@*#if(ImplementSeriLogLogging)
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
#endif*@

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region Add services to the container.

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
        
    @*#if(EnableSwaggerSupport)
    builder.Services.AddSwaggerGen();
    #endif*@

    builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("MyAmazingTemplateDatabaseConnection")
      ));

    @*#if(ImplementSeriLogLogging)
    var appLogger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.WithProperty("ServerName", Environment.MachineName) // whatever you need to log
        .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production")
        .Enrich.FromLogContext()
        .CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(appLogger);
    builder.Host.UseSerilog(appLogger);

    //debug DB Logging
    //Serilog.Debugging.SelfLog.Enable(msg =>
    //{
    //    Debug.Print(msg);
    //    Debugger.Break();
    //});
    #endif*@

    //Add parameters to log Http requests
    //builder.Services.AddHttpLogging();
    
    #endregion


    var app = builder.Build();

    #region Configure the HTTP request pipeline.
    app.Logger.LogInformation("Application is ready");

    @*#if(ImplementSeriLogLogging)
    //add details to log
    //app.UseSerilogRequestLogging(options =>
    //{
    //    // Customize the message template   
    //    options.MessageTemplate = "{RemoteIpAddress} {RequestScheme} {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

    //    // Emit debug-level events instead of the defaults
    //    options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

    //    // Attach additional properties to the request completion event
    //    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    //    {
    //        diagnosticContext.Set("UserName", httpContext.User.Identity.Name);
    //        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
    //        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
    //        diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
    //    };
    //});
    #endif*@

    //logg all HTTP Requests
    //app.UseHttpLogging();

    if (app.Environment.IsDevelopment())
    {
        @*#if(EnableSwaggerSupport)
        app.UseSwagger();
        app.UseSwaggerUI();
        #endif*@

        app.UseDeveloperExceptionPage();
    }

    //app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    #endregion
}
catch (Exception ex)
{
    @*#if(ImplementSeriLogLogging)
    Log.Fatal(ex, "Unhandled exception");
    #endif*@
}
finally
{
    @*#if(ImplementSeriLogLogging)
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
    #endif*@
}