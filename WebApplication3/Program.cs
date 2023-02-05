using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.Design;
using WebApplication3.Middleware;
using WebApplication3.Model;
using WebApplication3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>().AddErrorDescriber<ErrorDesc>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
{
	config.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
})
	.AddDefaultTokenProviders()
	.AddEntityFrameworkStores<AuthDbContext>()
    .AddErrorDescriber<ErrorDesc>();
builder.Services.Configure<IdentityOptions>(options =>
{
    //Password req
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 12;
    options.Password.RequiredUniqueChars = 1;

    //Unique email and Username check
    options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
});
builder.Services.AddDataProtection();
builder.Services.AddHttpClient();
//builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



//builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
//   opt.TokenLifespan = TimeSpan.FromHours(2));
builder.Services.AddTransient<IUserTwoFactorTokenProvider<ApplicationUser>, DataProtectorTokenProvider<ApplicationUser>>();
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
			   opt.TokenLifespan = TimeSpan.FromHours(2));
builder.Services.AddScoped<UserPasswordHistoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.UseMiddleware<AuthMiddleware>();




app.UseErrorHandlingMiddleware();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.Run();
