 

using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using Parc_Auto_v3.Services;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using Parc_Auto_v3.Models;
using Parc_Auto_v3.Services;
using Parc_Auto_v3.Utils;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Add this line to include Razor Pages


IConfigurationSection AppParameters = builder.Configuration.GetSection("AppParameters");
HelperDonGen.IsVesionDev = false;

HelperDonGen.MailServer = AppParameters.GetSection("MailServer").Value;
HelperDonGen.MailTo = AppParameters.GetSection("MailTo").Value;
HelperDonGen.MailFrom = AppParameters.GetSection("MailFrom").Value;
if (int.Parse(AppParameters.GetSection("IsVesionDev").Value) == 1)
    HelperDonGen.IsVesionDev = true;



// Register the services
builder.Services.AddScoped<IVisiteTechniqueService, VisiteTechniqueService>();
builder.Services.AddScoped<IVoitureService, VoitureService>();

builder.Services.AddScoped<IVignetteService, VignetteService>();

builder.Services.AddScoped<IVidangeService, VidangeService>();
builder.Services.AddScoped<ISinistreService, SinistreService>();
builder.Services.AddScoped<IAssuranceService, AssuranceService>();
builder.Services.AddScoped<IDemandesService, DemandesService>();
builder.Services.AddScoped<IMarqueService, MarqueService>();
builder.Services.AddScoped<IModeleService, ModeleService>();


// Configure the DbContext with the connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});



builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.Password.RequireDigit = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
   // app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();




// Set the license context
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Add this line to map Razor Pages


app.Run();
