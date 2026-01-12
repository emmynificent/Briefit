using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);


var port = Environment.GetEnvironmentVariable("PORT") ?? "5228";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");


// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  // This MUST come before building

builder.Services.AddScoped<IUrlService, UrlService>();


builder.Services.AddDbContext<BriefitDbContext>(options =>
{
    if (builder.Environment.IsProduction())
    {
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');
        var dbPort =uri.Port > 0 ? uri.Port : 5432;
        var database = uri.AbsolutePath.TrimStart('/');
        var connectionString = $"Host={uri.Host};Port={uri.dbPort};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";

        Console.WriteLine($"Connecting to database: {uri.Host}:{dbPort}/{database}")    ;    
        options.UseNpgsql(connectionString);
    }
    else
    {
        var connectionString = builder.Configuration.GetConnectionString("BriefConnectionString");
        options.UseSqlServer(connectionString);
    }
});


builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()   
        .AllowAnyHeader();
    });
});




var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();