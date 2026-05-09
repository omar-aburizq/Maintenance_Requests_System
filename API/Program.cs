using Application.Repositories;
using Application.Services.AuthService;
using Application.Services.CategoryService;
using Application.Services.CurrentUserService;
using Application.Services.RequestService;
using Application.Services.RoleService;
using Application.Services.UserService;
using Infrastructuer.Context;
using Infrastructuer.Data;
using Infrastructuer.Repositories;
using Infrastructuer.Services.CurrentUserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  // Install Microsoft.AspNetCore.Authentication.JwtBearer
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Maintenance_Requests_System API",
        Version = "v1"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Put **_ONLY_** your JWT Bearer token hera",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityReq = new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { } }
    };
    c.AddSecurityRequirement(securityReq);
});


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
builder.Services.AddScoped(typeof(IRoleService), typeof(RoleService));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
builder.Services.AddScoped(typeof(IRequestService), typeof(RequestService));
builder.Services.AddScoped(typeof(ICurrentUserService), typeof(CurrentUserService));
builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));


var app = builder.Build();

// Configure the HTTP request pipeline.

UserSeedData.UserSeed(app.Services);

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseStaticFiles(); // setup static file for internal

app.UseAuthentication();

app.UseAuthorization();

var uploadPath = builder.Configuration["FileStorage:UploadPath"]; // setup static file for external storage 

if (!string.IsNullOrEmpty(uploadPath))
{
    if (!Directory.Exists(uploadPath))
        Directory.CreateDirectory(uploadPath);

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(uploadPath),
        RequestPath = "/External"
    });
}

app.MapControllers();

app.Run();