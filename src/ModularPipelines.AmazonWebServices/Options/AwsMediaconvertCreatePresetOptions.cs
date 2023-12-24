using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconvert", "create-preset")]
public record AwsMediaconvertCreatePresetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--settings")] string Settings
) : AwsOptions
{
    [CommandSwitch("--category")]
    public string? Category { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}