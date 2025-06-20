using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

public class CustomWebApplicationFactory : WebApplicationFactory<Products.Backend.Api.Program>
{
    private readonly string _connectionString;

    public CustomWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            var dict = new Dictionary<string, string?>
            {
                {"ConnectionStrings:db-product-entities", _connectionString}
            };
            configBuilder.AddInMemoryCollection(dict);
        });
    }
}