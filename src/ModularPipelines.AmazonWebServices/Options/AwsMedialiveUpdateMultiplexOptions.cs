using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "update-multiplex")]
public record AwsMedialiveUpdateMultiplexOptions(
[property: CommandSwitch("--multiplex-id")] string MultiplexId
) : AwsOptions
{
    [CommandSwitch("--multiplex-settings")]
    public string? MultiplexSettings { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}