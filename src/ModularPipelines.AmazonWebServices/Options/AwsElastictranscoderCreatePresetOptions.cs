using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastictranscoder", "create-preset")]
public record AwsElastictranscoderCreatePresetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--container")] string Container
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--video")]
    public string? Video { get; set; }

    [CommandSwitch("--audio")]
    public string? Audio { get; set; }

    [CommandSwitch("--thumbnails")]
    public string? Thumbnails { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}