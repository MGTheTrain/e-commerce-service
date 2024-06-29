// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

using System.Reflection;
using Mgtt.ECom.Application.Services;
using Mgtt.ECom.Domain.OrderManagement;
using Mgtt.ECom.Domain.ProductManagement;
using Mgtt.ECom.Domain.ReviewManagement;
using Mgtt.ECom.Domain.ShoppingCart;
using Mgtt.ECom.Domain.UserManagement;
using Mgtt.ECom.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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
              Id = "Bearer"
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

builder.Services.AddTransient<IOrderItemService, OrderItemService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IReviewService, ReviewService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<ICartItemService, CartItemService>();
builder.Services.AddTransient<IUserService, UserService>();

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
app.UseAuthorization();

app.MapControllers();
app.Run();