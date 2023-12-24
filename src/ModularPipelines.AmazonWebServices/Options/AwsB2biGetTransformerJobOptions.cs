using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("b2bi", "get-transformer-job")]
public record AwsB2biGetTransformerJobOptions(
[property: CommandSwitch("--transformer-job-id")] string TransformerJobId,
[property: CommandSwitch("--transformer-id")] string TransformerId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}