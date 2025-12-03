using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "update", "(appservice-kube", "extension)")]
public record AzWebappUpdateAppserviceKubeExtensionOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--client-affinity-enabled")]
    public bool? ClientAffinityEnabled { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--minimum-elastic-instance-count")]
    public int? MinimumElasticInstanceCount { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--prewarmed-instance-count")]
    public int? PrewarmedInstanceCount { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}