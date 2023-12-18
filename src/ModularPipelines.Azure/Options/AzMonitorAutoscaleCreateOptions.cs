using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "autoscale", "create")]
public record AzMonitorAutoscaleCreateOptions(
[property: CommandSwitch("--count")] int Count,
[property: CommandSwitch("--resource")] string Resource
) : AzOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [BooleanCommandSwitch("--email-administrator")]
    public bool? EmailAdministrator { get; set; }

    [BooleanCommandSwitch("--email-coadministrators")]
    public bool? EmailCoadministrators { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--max-count")]
    public int? MaxCount { get; set; }

    [CommandSwitch("--min-count")]
    public int? MinCount { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-namespace")]
    public string? ResourceNamespace { get; set; }

    [CommandSwitch("--resource-parent")]
    public string? ResourceParent { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--scale-look-ahead-time")]
    public string? ScaleLookAheadTime { get; set; }

    [CommandSwitch("--scale-mode")]
    public string? ScaleMode { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}