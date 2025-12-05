using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "firewall-rules", "update")]
public record GcloudComputeFirewallRulesUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--allow")]
    public string[]? Allow { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--destination-ranges")]
    public string[]? DestinationRanges { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--[no-]enable-logging")]
    public string[]? NoEnableLogging { get; set; }

    [CliOption("--logging-metadata")]
    public string? LoggingMetadata { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--rules")]
    public string[]? Rules { get; set; }

    [CliOption("--source-ranges")]
    public string[]? SourceRanges { get; set; }

    [CliOption("--source-service-accounts")]
    public string[]? SourceServiceAccounts { get; set; }

    [CliOption("--source-tags")]
    public string[]? SourceTags { get; set; }

    [CliOption("--target-service-accounts")]
    public string[]? TargetServiceAccounts { get; set; }

    [CliOption("--target-tags")]
    public string[]? TargetTags { get; set; }
}