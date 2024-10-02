using BackEnd.Models;
using BackEnd.Repository;
using BackEnd.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using BackEnd.Repository.RepositoryImpl;
using BackEnd.Service.ServiceImpl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using BackEnd.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Authentication using JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:JwtIssuer"],
        ValidAudience = builder.Configuration["Jwt:JwtAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add DbContext
builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreContext")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder => builder.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:5175")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add Authentication and Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/user/login"; // Đường dẫn đến trang đăng nhập
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian hết hạn của cookie
        options.SlidingExpiration = true; // Tự động gia hạn cookie khi gần hết hạn
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("USER"));
});

// Dependency Injection for Repositories and Services
builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Order
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
// WishList
builder.Services.AddScoped<IWishListRepository, WishlistRepository>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
// PostCategory
builder.Services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
builder.Services.AddScoped<IPostCategoryService, PostCategoryService>();
// Post
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();
// Publisher
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
// Author 
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IJwtService, JwtService>(); // Đăng ký JWTService

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// Đăng ký Middleware kiểm tra token
app.UseMiddleware<TokenValidationMiddleware>();

// Apply CORS policy
app.UseCors("AllowSpecificOrigins");

// Enable authentication middleware
app.UseAuthentication();

// Enable authorization middleware
app.UseAuthorization();

app.MapControllers();

app.Run();