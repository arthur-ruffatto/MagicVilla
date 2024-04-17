using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Example of how to implement Serilog to log to a file instead of the built-in logger
//Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File("log/villalogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();
//builder.Host.UseSerilog();

builder.Services
    .AddControllers(option => { option.ReturnHttpNotAcceptable = true; })
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
