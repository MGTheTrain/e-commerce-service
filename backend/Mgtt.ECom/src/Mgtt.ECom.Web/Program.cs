// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Reflection;
using System.Text;
using Mgtt.ECom.Application.Services;
using Mgtt.ECom.Domain.OrderManagement;
using Mgtt.ECom.Domain.ProductManagement;
using Mgtt.ECom.Domain.ReviewManagement;
using Mgtt.ECom.Domain.ShoppingCart;
using Mgtt.ECom.Infrastructure.Connectors;
using Mgtt.ECom.Infrastructure.Settings;
using Mgtt.ECom.Persistence.DataAccess;
using Mgtt.ECom.Web.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Domain"];
    options.Audience = builder.Configuration["Auth0:Audience"];
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("manage:products", policy =>
        policy.Requirements.Add(new HasPermissionRequirement("manage:products")));
    options.AddPolicy("manage:orders", policy =>
        policy.Requirements.Add(new HasPermissionRequirement("manage:orders")));
    options.AddPolicy("manage:reviews", policy =>
        policy.Requirements.Add(new HasPermissionRequirement("manage:reviews")));
    options.AddPolicy("manage:carts", policy =>
        policy.Requirements.Add(new HasPermissionRequirement("manage:carts")));
    options.AddPolicy("manage:own-product", policy =>
        policy.Requirements.Add(new HasPermissionRequirement("manage:own-product")));
    options.AddPolicy("manage:own-order", policy =>
        policy.Requirements.Add(new HasPermissionRequirement("manage:own-order")));
    options.AddPolicy("manage:own-review", policy =>
        policy.Requirements.Add(new HasPermissionRequirement("manage:own-review")));
    options.AddPolicy("manage:own-cart", policy =>
        policy.Requirements.Add(new HasPermissionRequirement("manage:own-cart")));

    options.AddPolicy("manage:products-and-own-product", policy =>
    {
        policy.RequireClaim("permissions", "manage:products", "manage:own-product");
    });

    options.AddPolicy("manage:reviews-and-own-review", policy =>
    {
        policy.RequireClaim("permissions", "manage:reviews", "manage:own-review");
    });

    options.AddPolicy("manage:carts-and-own-cart", policy =>
    {
        policy.RequireClaim("permissions", "manage:carts", "manage:own-cart");
    });

    options.AddPolicy("manage:orders-and-own-order", policy =>
    {
        policy.RequireClaim("permissions", "manage:orders", "manage:own-order");
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, HasPermissionHandler>();

var customCORSName = "AllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: customCORSName,
        policy =>
                        {
                            policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "e-commerce-service",
        Version = "v1",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Marvin Gajek",
            Email = "placeholder@gmail.com",
        },
        License = new OpenApiLicense
        {
            Name = "MgTheTrain License",
            Url = new Uri("https://github.com/MGTheTrain/multi-media-management-service"),
        },
        Description = @"API documentation for the e-commerce-service",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Add JWT with Bearer here",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
    {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference
          {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer",
          },
        },
        new string[] { }
    },
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var psqlDbconnectionString = builder.Configuration.GetConnectionString("PsqlDatabase");
var dbContext = builder.Services.AddDbContext<DbContext, PsqlDbContext>(options =>
{
    options.UseNpgsql(psqlDbconnectionString);
});


builder.Services.Configure<PayPalSettings>(builder.Configuration.GetSection("PayPalSettings"));
builder.Services.AddHttpClient<IPayPalConnector, PayPalConnector>();

builder.Services.AddTransient<IOrderItemService, OrderItemService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IReviewService, ReviewService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<ICartItemService, CartItemService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PsqlDbContext>();
    context.Database.Migrate();
}

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();

// }

// Incorporate additional middleware if necessary
app.UseRouting();
app.UseCors(customCORSName);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();