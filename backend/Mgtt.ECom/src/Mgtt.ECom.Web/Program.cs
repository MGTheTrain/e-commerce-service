// Incorporate required dependencies from the infrastructure, persistence, domain and application layer

var builder = WebApplication.CreateBuilder(args);

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
            Email = "marvin.gajek@outlook.com"
        },
        License = new OpenApiLicense
        {
            Name = "MgTheTrain License",
            Url = new Uri("https://github.com/MGTheTrain/multi-media-management-service")
        },
        Description = @"API documentation for the e-commerce-service",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Add JWT with Bearer here",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference
          {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer"
          }
          },
          new string[] { }
        }
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Incorporate additional middleware if necessary

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.Run();