using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "firewall-rules", "update")]
public record GcloudComputeFirewallRulesUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--allow")]
    public string[]? Allow { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--destination-ranges")]
    public string[]? DestinationRanges { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--[no-]enable-logging")]
    public string[]? NoEnableLogging { get; set; }

    [CommandSwitch("--logging-metadata")]
    public string? LoggingMetadata { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--rules")]
    public string[]? Rules { get; set; }

    [CommandSwitch("--source-ranges")]
    public string[]? SourceRanges { get; set; }

    [CommandSwitch("--source-service-accounts")]
    public string[]? SourceServiceAccounts { get; set; }

    [CommandSwitch("--source-tags")]
    public string[]? SourceTags { get; set; }

    [CommandSwitch("--target-service-accounts")]
    public string[]? TargetServiceAccounts { get; set; }

    [CommandSwitch("--target-tags")]
    public string[]? TargetTags { get; set; }
}