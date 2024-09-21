using BackEnd.Models;
using BackEnd.Repository;
using BackEnd.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;  

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

// Dependency Injection for Repositories and Services
builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();

//order
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

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

// Apply CORS policy
app.UseCors("AllowSpecificOrigins");

// Enable authentication middleware
app.UseAuthentication();

// Enable authorization middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
