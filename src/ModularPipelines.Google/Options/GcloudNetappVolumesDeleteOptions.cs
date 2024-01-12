using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "volumes", "delete")]
public record GcloudNetappVolumesDeleteOptions(
[property: PositionalArgument] string Volume,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}