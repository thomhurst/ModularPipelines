using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "wait", "multiplex-deleted")]
public record AwsMedialiveWaitMultiplexDeletedOptions(
[property: CommandSwitch("--multiplex-id")] string MultiplexId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}