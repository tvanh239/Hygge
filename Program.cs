using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Hygge.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Hygge.Service;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("HyggeContextConnection") ?? throw new InvalidOperationException("Connection string 'HyggeContextConnection' not found.");

builder.Services.AddDbContext<HyggeContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<HyggeUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<HyggeContext>();

builder.Services.AddScoped<IEmailService, EmailService>();


// Add services to the container.
builder.Services.AddRazorPages();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
