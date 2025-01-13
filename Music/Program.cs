using Dal.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Music.Helpers;
using Music.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
if (jwtSettings == null)
    throw new Exception("Jwt settings not found");

// Configure JWT Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true, // Validate the token's issuer
            ValidateAudience = true, // Validate the token's audience
            ValidateLifetime = true, // Ensure the token hasn't expired
            ValidateIssuerSigningKey = true, // Validate the signing key
            ValidIssuer = jwtSettings.Issuer, // Replace with your issuer
            ValidAudience = jwtSettings.Audience, // Replace with your audience
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(jwtSettings.Key)) // Replace with your secure key
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                // Log the reason for authentication failure
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                // Log successful token validation
                Console.WriteLine($"Token validated for user: {context.Principal.Identity?.Name}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                // Log when a challenge occurs (e.g., invalid or missing token)
                Console.WriteLine("Token challenge triggered.");
                Console.WriteLine($"Error: {context.Error}");
                Console.WriteLine($"Error Description: {context.ErrorDescription}");
                return Task.CompletedTask;
            }
        };
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddDal();
builder.Services.AddServices();
builder.Logging.AddConsole();
// Add Swagger services with Bearer token authentication
builder.Services.AddSwaggerGen(options =>
{
    // Add Bearer authentication scheme to Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: 'Bearer eyJhb...'"
    });

    // Apply the Bearer authentication scheme globally
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>() // No specific scopes required
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger(); // Enables Swagger middleware
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger"; // Access at /swagger
    });
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapControllers();

app.Run();