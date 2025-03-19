using FirstWebAPIProject.Data;  
using FirstWebAPIProject.Mappings; 
using FirstWebAPIProject.Middlewares; 
using FirstWebAPIProject.Repository.Implementation;  
using FirstWebAPIProject.Repository.Interface;  
using Microsoft.AspNetCore.Authentication.JwtBearer;  
using Microsoft.AspNetCore.Identity; 
using Microsoft.EntityFrameworkCore;  
using Microsoft.Extensions.FileProviders;  
using Microsoft.IdentityModel.Tokens;  
using Microsoft.OpenApi.Models;  
using Serilog;  
using System.Text;  

var builder = WebApplication.CreateBuilder(args);  // Creates a new WebApplication instance 

var logger = new LoggerConfiguration() // Configures Serilog logger 
    .WriteTo.Console()  // Writes logs to the console
    .WriteTo.File("Logs/Walks_Log.txt", rollingInterval: RollingInterval.Minute) // Writes logs to a file 
    .MinimumLevel.Warning()  // Minimum log level is Warning
    .CreateLogger();  // Creates the logger 

builder.Logging.ClearProviders();  
builder.Logging.AddSerilog(logger); 

builder.Services.AddControllers(); // Adds controller services
builder.Services.AddHttpContextAccessor();  // Adds HTTP context accessor services 

// Configure Swagger (API documentation)

builder.Services.AddEndpointsApiExplorer(); // Adds API explorer services for endpoint discovery 
builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Walks API ", Version = "v1" });

    // Configure JWT authentication in Swagger

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme 
    {
        Name = "Authorization", // Name of the header containing the JWT token 
        In = ParameterLocation.Header, // JWT token will be passed in the request header
        Type = SecuritySchemeType.ApiKey, // Type of security scheme is API key
        Scheme = JwtBearerDefaults.AuthenticationScheme // Scheme is the authentication scheme (Bearer)
    });

    // Set up security requirements for Swagger to use JWT tokens

    options.AddSecurityRequirement(new OpenApiSecurityRequirement  
    {
        {
            new OpenApiSecurityScheme  
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, // Reference type is SecurityScheme (JWT token) 
                    Id = JwtBearerDefaults.AuthenticationScheme // Id is the authentication scheme (Bearer)
                },
                Scheme = "Oauth2", // Scheme is Oauth2 
                Name = JwtBearerDefaults.AuthenticationScheme, // Name is the authentication scheme (Bearer)
                In = ParameterLocation.Header //    JWT token will be passed in the request header
            },
            new List<string>()
        }
    });
});

// Configure database connections
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbCon"))); // Connects to main database

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConn"))); // Connects to authentication database

// Register repository services for dependency injection
builder.Services.AddScoped<IRegionRepository, RegionRepository>(); 
builder.Services.AddScoped<IWalksRepository, WalksRepository>(); // Walks repository
builder.Services.AddScoped<ITokenRepository, TokenRepository>(); // Token repository
builder.Services.AddScoped<IImageRepository, ImageRepository>(); // Image repository

// Configure AutoMapper for object mapping
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles)); 

// Configure Identity for user authentication

builder.Services.AddIdentityCore<IdentityUser>() 
    .AddRoles<IdentityRole>() // Adds role-based authentication
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("WalksAPI") 
    .AddEntityFrameworkStores<AuthDbContext>() // Stores user data in the AuthDbContext
    .AddDefaultTokenProviders(); // Adds default token providers for generating reset tokens, etc.

// Configure Identity options (password rules)
builder.Services.Configure<IdentityOptions>(options => 
{
    options.Password.RequireDigit = false; // No number required
    options.Password.RequireLowercase = false; // No lowercase required
    options.Password.RequireNonAlphanumeric = false; // No special character required
    options.Password.RequireUppercase = false; // No uppercase required
    options.Password.RequiredLength = 6; // Minimum password length is 6
    options.Password.RequiredUniqueChars = 1; // At least 1 unique character
});

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) 
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Validate the issuer of the token
        ValidateAudience = true, // Validate the audience of the token
        ValidateLifetime = true, // Validate token expiry
        ValidateIssuerSigningKey = true, // Validate signing key
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Issuer from config
        ValidAudience = builder.Configuration["Jwt:Audience"], // Audience from config
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Signing key
    });

var app = builder.Build(); // Builds the application with configured services

// Configure middleware pipeline (order matters)
if (app.Environment.IsDevelopment()) // If running in development mode
{
    app.UseSwagger(); // Enables Swagger UI
    app.UseSwaggerUI(); // Displays Swagger UI
}

app.UseMiddleware<ExceptionHandlerMiddleware>(); // Custom middleware for handling exceptions globally

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS

app.UseAuthentication(); // Enables authentication middleware
app.UseAuthorization(); // Enables authorization middleware

// Configure serving of static files (images)
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")), 
    RequestPath = "/Images" // URL path to access images
});

app.MapControllers(); // Maps controllers to handle API requests

app.Run(); // Runs the application
