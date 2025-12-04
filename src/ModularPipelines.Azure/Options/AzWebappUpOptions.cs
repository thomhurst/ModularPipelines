using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "up")]
public record AzWebappUpOptions : AzOptions
{
    [CliOption("--app-service-environment")]
    public string? AppServiceEnvironment { get; set; }

    [CliFlag("--dryrun")]
    public bool? Dryrun { get; set; }

    [CliFlag("--html")]
    public bool? Html { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--launch-browser")]
    public bool? LaunchBrowser { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--logs")]
    public bool? Logs { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliOption("--plan")]
    public string? Plan { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--runtime")]
    public string? Runtime { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}