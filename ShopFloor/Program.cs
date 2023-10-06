using ShopFloor.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ShopFloor.EFModels;
using ShopFloor.Service;
using Microsoft.EntityFrameworkCore;
using Blazored.Toast;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


builder.Services.AddDevExpressBlazor(options => {
	options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
	options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
});

builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();



builder.Services.AddDbContextFactory<ShopFloorDBContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddBlazoredToast();

builder.Services.AddSingleton<TagService>();
builder.Services.AddSingleton<TagUpdateService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<TagUpdateService>());

builder.Services.AddSingleton<ConfigService>();

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

app.UseRouting();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();