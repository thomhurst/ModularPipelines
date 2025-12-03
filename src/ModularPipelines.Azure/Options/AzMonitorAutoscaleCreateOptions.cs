using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "autoscale", "create")]
public record AzMonitorAutoscaleCreateOptions(
[property: CliOption("--count")] int Count,
[property: CliOption("--resource")] string Resource
) : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliFlag("--email-administrator")]
    public bool? EmailAdministrator { get; set; }

    [CliFlag("--email-coadministrators")]
    public bool? EmailCoadministrators { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--max-count")]
    public int? MaxCount { get; set; }

    [CliOption("--min-count")]
    public int? MinCount { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-namespace")]
    public string? ResourceNamespace { get; set; }

    [CliOption("--resource-parent")]
    public string? ResourceParent { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--scale-look-ahead-time")]
    public string? ScaleLookAheadTime { get; set; }

    [CliOption("--scale-mode")]
    public string? ScaleMode { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}