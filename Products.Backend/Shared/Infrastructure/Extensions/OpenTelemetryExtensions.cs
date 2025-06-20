using FastEndpoints.OpenTelemetry;
using Npgsql;
using OpenTelemetry;

namespace Products.Backend.Infrastructure.Extensions;

public static class OpenTelemetryExtensions
{
    public static OpenTelemetryBuilder ConnectBackendServices(this OpenTelemetryBuilder builder) =>
        builder
            .WithTracing(static tracing =>
            {
                tracing.AddFastEndpointsInstrumentation();
                tracing.AddNpgsql();
            })
            .WithMetrics(static metrics =>
            {
                metrics.AddNpgsqlInstrumentation();
            });
}