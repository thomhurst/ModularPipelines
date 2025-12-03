using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "env", "dapr-component", "resiliency", "create")]
public record AzContainerappEnvDaprComponentResiliencyCreateOptions(
[property: CliOption("--dapr-component-name")] string DaprComponentName,
[property: CliOption("--environment")] string Environment,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--in-http-delay")]
    public string? InHttpDelay { get; set; }

    [CliOption("--in-http-interval")]
    public string? InHttpInterval { get; set; }

    [CliOption("--in-http-retries")]
    public string? InHttpRetries { get; set; }

    [CliOption("--in-timeout")]
    public string? InTimeout { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--out-http-delay")]
    public string? OutHttpDelay { get; set; }

    [CliOption("--out-http-interval")]
    public string? OutHttpInterval { get; set; }

    [CliOption("--out-http-retries")]
    public string? OutHttpRetries { get; set; }

    [CliOption("--out-timeout")]
    public string? OutTimeout { get; set; }

    [CliOption("--yaml")]
    public string? Yaml { get; set; }
}