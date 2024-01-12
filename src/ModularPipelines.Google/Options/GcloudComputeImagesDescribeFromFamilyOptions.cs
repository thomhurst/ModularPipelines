using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "images", "describe-from-family")]
public record GcloudComputeImagesDescribeFromFamilyOptions(
[property: PositionalArgument] string ImageName
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}