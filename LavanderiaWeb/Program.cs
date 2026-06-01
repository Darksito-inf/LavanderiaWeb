using LavanderiaWeb.Data;

var builder = WebApplication.CreateBuilder(args);

string rutaBase = builder.Environment.ContentRootPath;
AppDomain.CurrentDomain.SetData("DataDirectory", rutaBase);

builder.Services.AddRazorPages();

// Registro de DAOs individuales
builder.Services.AddSingleton<ClienteDAO>();
builder.Services.AddSingleton<SubscripcionDAO>();
builder.Services.AddSingleton<TipoPrendaDAO>();
builder.Services.AddSingleton<OrdenServicioDAO>();
builder.Services.AddSingleton<PrendaOrdenDAO>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();