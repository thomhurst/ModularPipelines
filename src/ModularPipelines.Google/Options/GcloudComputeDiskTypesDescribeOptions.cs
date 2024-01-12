using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "disk-types", "describe")]
public record GcloudComputeDiskTypesDescribeOptions(
[property: PositionalArgument] string DiskType
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}