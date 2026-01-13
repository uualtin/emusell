using Emusell.Services;
using Emusell.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MongoDB Configuration
builder.Services.AddSingleton<MongoDbService>();

// Application Services
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<OffcanvasService>();
builder.Services.AddScoped<LocalizationService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Seed default admin user
await SeedAdminUser(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<Emusell.App>()
    .AddInteractiveServerRenderMode();

app.Run();

// Seed Admin User Method
async Task SeedAdminUser(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<UserService>();
    
    // Check if admin exists
    var adminEmail = "admin@emusell.com";
    var existingAdmin = await userService.GetUserByEmailAsync(adminEmail);
    
    if (existingAdmin == null)
    {
        var admin = new User
        {
            Email = adminEmail,
            PasswordHash = "Admin123!", // Will be hashed in CreateUserAsync
            FullName = "Sistem Yöneticisi",
            Phone = "0500 000 00 00",
            Role = UserRole.Admin,
            IsActive = true
        };
        
        await userService.CreateUserAsync(admin);
        Console.WriteLine("✅ Default admin user created: admin@emusell.com / Admin123!");
    }
    else
    {
        Console.WriteLine("ℹ️ Admin user already exists.");
    }
}
