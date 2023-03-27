using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using WasteZero.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WasteZero.Services;
using Radzen;
using System.Net;
//using System.Net;


var builder = WebApplication.CreateBuilder(args); 
var connectionString = builder.Configuration.GetConnectionString("ProjectDB");
IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
IPAddress? localIp = addresses.FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();
builder.Services.AddHttpClient();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddDbContextFactory<WasteZeroDbContext>(options => options.UseSqlite(connectionString)); 
builder.Services.AddDbContext<WasteZeroDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductTypeService>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
