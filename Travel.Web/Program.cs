using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Register HttpClient
builder.Services.AddHttpClient();

// Enable session
builder.Services.AddSession();


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {

            ValidateIssuer = true,
            ValidIssuer = "TravelApi",

            ValidateAudience = true,
            ValidAudience = "TravelApiUsers",

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this_is_a_super_secret_key_1234567890!!")),
        };
    });

builder.Services.AddAuthorization();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// before app.UseRouting() has to be
app.UseSession(); 

app.UseRouting();

// checks if jwt token exists
// if it is null it redirects to Login page
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("JWToken");

    if (string.IsNullOrEmpty(token))
    {
        Console.WriteLine("programm: JWToken is missing — redirecting to /Account/Login");

        var path = context.Request.Path.Value?.ToLower();
        var isHtmlPage = path != null && !path.StartsWith("/api") && path.EndsWith(".cshtml") == false;

        if (isHtmlPage && !path.Contains("/account/login"))
        {
            context.Response.Redirect("/Account/Login");
            return; // Stop further processing
        }
    }
    else
    {
        Console.WriteLine($"programm: JWToken = {token}");
        context.Request.Headers["Authorization"] = $"Bearer {token}";
    }

    await next();
});


app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
