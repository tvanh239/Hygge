//*****************************************************************************
//* ALL RIGHTS RESERVED. COPYRIGHT (C) 2024 Hygge                             *
//*****************************************************************************
//* File Name    : Login.cshtml.cs   　　　                        　          *
//* Function     : File Main                                                  *
//* Create       : VietAnh 2023/12/28                                         *
//*****************************************************************************.
using Microsoft.EntityFrameworkCore;
using Hygge.Data;
using Hygge.Service;

var builder = WebApplication.CreateBuilder(args);

// Connect database
var connectionString = builder.Configuration.GetConnectionString("HyggeContextConnection") ?? throw new InvalidOperationException("Connection string 'HyggeContextConnection' not found.");

builder.Services.AddDbContext<HyggeContext>(options => options.UseSqlServer(connectionString));

// Add Identity for user
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
