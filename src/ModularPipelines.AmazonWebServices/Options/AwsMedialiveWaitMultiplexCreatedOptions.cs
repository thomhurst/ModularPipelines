using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "wait", "multiplex-created")]
public record AwsMedialiveWaitMultiplexCreatedOptions(
[property: CommandSwitch("--multiplex-id")] string MultiplexId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}