using Community.Microsoft.Extensions.Caching.PostgreSql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Siyasett.Data;
using Siyasett.Data.Data;
using Siyasett.Web.Languages;
using Siyasett.Web.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);



AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder
    .UseNetTopologySuite();
//    .UseLoggerFactory(loggerFactory) // Configure ADO.NET logging
//    .UsePeriodicPasswordProvider(); // Automatically rotate the password periodically
await using var dataSource = dataSourceBuilder.Build();


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(dataSource, opt => opt.UseNetTopologySuite()));

builder.Services.AddMemoryCache();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options =>
{

    options.SignIn.RequireConfirmedAccount = true;
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
    options.User.RequireUniqueEmail = true;


    options.SignIn.RequireConfirmedEmail = true;

})
    .AddRoles<IdentityRole<Guid>>()
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddDefaultTokenProviders().AddDefaultUI();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));


var emailConfig = builder.Configuration.GetSection("MailSettings").Get<MailSettings>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailService>();


builder.Services.AddDistributedPostgreSqlCache(setup =>
{
    setup.ConnectionString = connectionString;
    setup.SchemaName = builder.Configuration["PgCache:SchemaName"];
    setup.TableName = builder.Configuration["PgCache:TableName"];
    setup.CreateInfrastructure = !string.IsNullOrWhiteSpace(builder.Configuration["PgCache:CreateInfrastructure"]);
    setup.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(60);

    // CreateInfrastructure is optional, default is TRUE
    // This means que every time starts the application the 
    // creation of table and database functions will be verified.
});
builder.Services.AddSession(option => option.IdleTimeout = TimeSpan.FromMinutes(30));

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Forbiden");
    options.Cookie.Name = "Cookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
    options.LoginPath = new PathString("/Login");
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});


builder.Services.AddLocalization();
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
    options.ModelBinderProviders.Insert(0, new DoubleModelBinderProvider());
    options.ModelBinderProviders.Insert(0, new FloatModelBinderProvider());
}).AddViewLocalization();

builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<ILocalizationService, LocalizationService>();

var serviceProvider = builder.Services.BuildServiceProvider();
var languageService = serviceProvider.GetRequiredService<ILanguageService>();
var languages = languageService.GetLanguages();
var cultures = languages.Select(x => new CultureInfo(x.Culture)).ToArray();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var turkishCulture = cultures.FirstOrDefault(x => x.Name == "tr-TR");
    options.DefaultRequestCulture = new RequestCulture(turkishCulture?.Name ?? "tr-TR");
    options.SetDefaultCulture(turkishCulture?.Name ?? "tr-TR");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

//var supportedCultures = new[] {"tr-TR", "en-US" };
//var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
//    .AddSupportedCultures(supportedCultures)
//    .AddSupportedUICultures(supportedCultures);


app.UseRequestLocalization();

app.MapControllerRoute(name: "areas",pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(name: "default",pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
