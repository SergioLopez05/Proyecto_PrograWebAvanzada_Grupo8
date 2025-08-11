using EventosCR.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient("Api", c =>
{
    c.BaseAddress = new Uri("https://localhost:7175/"); // URL de tu API
});

builder.Services.AddHttpClient<ApiBoletoService>();
builder.Services.AddTransient<ApiAuthService>();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";  // Ruta a la página de login
        options.AccessDeniedPath = "/Account/AccessDenied"; // Opcional
        options.Cookie.Name = "EventosCostaRicaAuth";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });


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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Eventos}/{action=Index}/{id?}");

app.Run();
