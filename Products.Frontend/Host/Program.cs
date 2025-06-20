using System.Net;
using BitzArt.Blazor.Auth.Server;
using Ixnas.AltchaNet;
using Products.Frontend.ClientShared.Extensions;
using Products.Frontend.Host.Components;
using Products.PublicApi.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseDefaultServiceProvider(static (c, opts) =>
{
    opts.ValidateScopes = true;
    opts.ValidateOnBuild = true;
});

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();

var container = new CookieContainer();
builder.Services.AddSingleton(container);

builder.Services.AddSingleton(static sp => Altcha.CreateSolverBuilder().Build());
builder.Services.AddClient();
builder.Services.ConfigureProductsBackendApiApizrManagers(options =>
	options.WithHttpClientHandler(sp => new()
	{
		CookieContainer = container,
        UseCookies = true
	}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(
		typeof(Products.Frontend.Client.Program).Assembly,
		typeof(Products.Frontend.ProductsApi.Extensions.ServiceCollectionExtensions).Assembly,
		typeof(Products.Frontend.SharedComponents.Extensions.ServiceCollectionExtensions).Assembly);

app.MapAuthEndpoints();

await app.RunAsync();