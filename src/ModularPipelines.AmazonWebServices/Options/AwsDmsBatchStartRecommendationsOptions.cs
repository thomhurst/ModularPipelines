using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "batch-start-recommendations")]
public record AwsDmsBatchStartRecommendationsOptions : AwsOptions
{
    [CommandSwitch("--data")]
    public string[]? Data { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}