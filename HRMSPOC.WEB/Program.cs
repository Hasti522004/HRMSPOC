using HRMSPOC.WEB.Controllers;
using HRMSPOC.WEB.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7095");
})
.AddHttpMessageHandler<AuthHttpClientHandler>();
builder.Services.AddHttpClient<OrganizationService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7095");
})
.AddHttpMessageHandler<AuthHttpClientHandler>();
builder.Services.AddHttpClient<UserService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7095");
})
.AddHttpMessageHandler<AuthHttpClientHandler>();
builder.Services.AddHttpClient<EmployeeService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7095");
})
.AddHttpMessageHandler<AuthHttpClientHandler>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<AuthHttpClientHandler>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7095";
        options.RequireHttpsMetadata = false;
        options.Audience = "api";
    });

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("SuperAdmin"));
//    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("HROnly", policy => policy.RequireRole("HR"));
//    options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"));
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
