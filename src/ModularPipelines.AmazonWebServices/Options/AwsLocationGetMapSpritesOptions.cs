using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "get-map-sprites")]
public record AwsLocationGetMapSpritesOptions(
[property: CommandSwitch("--file-name")] string FileName,
[property: CommandSwitch("--map-name")] string MapName
) : AwsOptions
{
    [CommandSwitch("--key")]
    public string? Key { get; set; }
}