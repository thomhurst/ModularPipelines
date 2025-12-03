using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "ingress", "enable")]
public record AzContainerappIngressEnableOptions(
[property: CliOption("--target-port")] string TargetPort,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliFlag("--allow-insecure")]
    public bool? AllowInsecure { get; set; }

    [CliOption("--exposed-port")]
    public string? ExposedPort { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--transport")]
    public string? Transport { get; set; }
}