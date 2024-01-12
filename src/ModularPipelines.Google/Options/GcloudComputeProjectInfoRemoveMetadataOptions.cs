using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "project-info", "remove-metadata")]
public record GcloudComputeProjectInfoRemoveMetadataOptions : GcloudOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--keys")]
    public string[]? Keys { get; set; }
}