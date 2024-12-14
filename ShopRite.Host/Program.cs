using ShopRite.Infrastructure.DependencyInjection;
using ShopRite.Application.DependencyInjection;
using ShopRite.Host.Filter;
using Serilog;



    var builder = WebApplication.CreateBuilder(args);

    #region LoggerConfiguration
    Log.Logger = new LoggerConfiguration()
					 .Enrich.FromLogContext()
					 .WriteTo.Console()
					 .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
					 .CreateLogger();
	builder.Host.UseSerilog();
Log.Information("Shoprite starting up...");
	#endregion


	// Add services to the container.
	#region Service 

	builder.Services.AddControllers()
	.ConfigureApiBehaviorOptions(options =>
	{

		options.SuppressModelStateInvalidFilter = true;
	});//for custom message  different from the workload

	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	builder.Services.AddScoped<RequestAuthActionFilterAttribute>();


	builder.Services.AddInfraustureService(builder.Configuration);
	builder.Services.AddApplicationService();
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.AllowAnyHeader()
			  .AllowAnyMethod()
              //.WithOrigins("https://localhost:7233") allow only request from this server or IP
              .AllowAnyOrigin(); // this is open to any server or IP
			  //.AllowCredentials();//we can pass a token here
	});
});

#endregion

try
{
    #region Middlewear

    var app = builder.Build();
	app.UseCors();
	app.UseSerilogRequestLogging();
	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}
	app.UseInfrastructureService();
	app.UseHttpsRedirection();

	app.UseAuthorization();

	app.MapControllers();

	app.Run();
	#endregion
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    Log.Fatal(ex,$"Shoprite failed to start corretly , Host terminated unexpectedly {type}");
}
finally
{
     Log.CloseAndFlush();
}
