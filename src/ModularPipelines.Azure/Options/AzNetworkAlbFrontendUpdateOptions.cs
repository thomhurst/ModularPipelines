using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "alb", "frontend", "update")]
public record AzNetworkAlbFrontendUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--alb-name")]
    public string? AlbName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--frontend-name")]
    public string? FrontendName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}