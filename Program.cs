using Microsoft.EntityFrameworkCore;
using radio_waves.Data;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

// SEED ROLES AND USERS
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Admin", "Receptionist" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed Admin user
    var adminEmail = "admin@example.com";
    var adminPassword = "Admin123!";

    if (await userManager.FindByEmailAsync(adminEmail) is null)
    {
        var adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    // Seed Receptionist user
    var receptionistEmail = "reception@lab.com";
    var receptionistPassword = "Reception123!";

    if (await userManager.FindByEmailAsync(receptionistEmail) is null)
    {
        var receptionistUser = new IdentityUser
        {
            UserName = receptionistEmail,
            Email = receptionistEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(receptionistUser, receptionistPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(receptionistUser, "Receptionist");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Set default route to login page
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
