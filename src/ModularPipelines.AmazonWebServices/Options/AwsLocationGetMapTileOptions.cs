using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "get-map-tile")]
public record AwsLocationGetMapTileOptions(
[property: CommandSwitch("--map-name")] string MapName,
[property: CommandSwitch("--x")] string X,
[property: CommandSwitch("--y")] string Y,
[property: CommandSwitch("--z")] string Z
) : AwsOptions
{
    [CommandSwitch("--key")]
    public string? Key { get; set; }
}