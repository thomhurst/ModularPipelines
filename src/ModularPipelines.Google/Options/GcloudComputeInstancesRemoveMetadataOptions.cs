using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "remove-metadata")]
public record GcloudComputeInstancesRemoveMetadataOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--keys")]
    public string[]? Keys { get; set; }
}