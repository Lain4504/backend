using BackEnd.Models;
using BackEnd.Repository;
using BackEnd.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using BackEnd.Middlewares;
using VNPAY_CS_ASPX;
using BackEnd.Repository.RepositoryImpl;
using BackEnd.Service.ServiceImpl;

// Khởi tạo ứng dụng
var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ vào container
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình xác thực JWT Bearer
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"];
    options.ClientSecret = builder.Configuration["Google:ClientSecret"];
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminRole", policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("UserRole", policy => policy.RequireRole("USER"));
});
// Thêm DbContext
builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("BookStoreContext"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("BookStoreContext"))));

// CORS: cho phép các nguồn cụ thể
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder => builder.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:5001", "http://localhost:5000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Authorization")); // Cho phép expose Authorization header nếu cần
});

// Thêm các dịch vụ và repository
builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
builder.Services.AddScoped<IPostCategoryService, PostCategoryService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IJwtService, JwtService>(); // Register JWTService
builder.Services.AddScoped<IFeedBackRepository, FeedBackRepository>();
builder.Services.AddScoped<IFeedBackService,FeedBackService>();
builder.Services.AddSignalR();
builder.Services.AddScoped<IWishListRepository, WishlistRepository>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<Payment, Payment>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAdsRepository, AdsRepository>();
builder.Services.AddScoped<IAdsService, AdsService>();
builder.Services.AddScoped<IChartRepository, ChartRepository>();
builder.Services.AddScoped<IChartService, ChartService>();
var app = builder.Build();

// Cấu hình pipeline HTTP request
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var errorResponse = new
            {
                message = "Đã xảy ra lỗi!",
                detail = exceptionHandlerPathFeature?.Error.Message
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        });
    });
}

app.UseHttpsRedirection();
app.UseRouting();

// Áp dụng CORS
app.UseCors("AllowSpecificOrigins");

// Kích hoạt middleware xác thực
app.UseAuthentication();
app.UseAuthorization();

// Đăng ký middleware xác thực token (nếu có)
app.UseMiddleware<TokenValidationMiddleware>();


// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers();
//     endpoints.MapHub<CommentHub>("/commentHub");
// });
app.MapControllers();
app.MapHub<CommentHub>("/commentHub");
app.Run();
