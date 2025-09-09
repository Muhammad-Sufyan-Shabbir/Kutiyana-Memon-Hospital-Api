using Kutiyana_Memon_Hospital_Api.API.AutoMapper;
using Kutiyana_Memon_Hospital_Api.API.Data;
using Kutiyana_Memon_Hospital_Api.API.Extensions;
using Kutiyana_Memon_Hospital_Api.API.MiddleWare;
using Kutiyana_Memon_Hospital_Api.API.Services.Implementation;
using Kutiyana_Memon_Hospital_Api.API.Services.Interfaces;
using Kutiyana_Memon_Hospital_Api.API.UnitOfWork.Implementation;
using Kutiyana_Memon_Hospital_Api.API.UnitOfWork.Interfaces;
using Kutiyana_Memon_Hospital_Api.Repositories.Implementation;
using Kutiyana_Memon_Hospital_Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Set wwwroot for static files
builder.WebHost.UseWebRoot("wwwroot");

// Controllers
builder.Services.AddControllers();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(MappingProfile));

// DbContext (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Register custom services + UnitOfWork
builder.Services.RegisterServices();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFunctionRepository, FunctionRepository>();

// ✅ Register EmailService for DI
builder.Services.AddTransient<IEmailService, EmailService>();

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// ✅ CORS for Angular App
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular ka port yahan
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Swagger + JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kutiyana Memon Hospital API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }
    });
});

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
