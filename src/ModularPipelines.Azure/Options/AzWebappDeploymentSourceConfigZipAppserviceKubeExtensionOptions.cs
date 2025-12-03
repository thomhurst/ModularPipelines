using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "deployment", "source", "config-zip", "(appservice-kube", "extension)")]
public record AzWebappDeploymentSourceConfigZipAppserviceKubeExtensionOptions(
[property: CliOption("--src")] string Src
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--is-kube")]
    public bool? IsKube { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}