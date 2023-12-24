using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-invalidation")]
public record AwsCloudfrontCreateInvalidationOptions(
[property: CommandSwitch("--distribution-id")] string DistributionId
) : AwsOptions
{
    [CommandSwitch("--invalidation-batch")]
    public string? InvalidationBatch { get; set; }

    [CommandSwitch("--paths")]
    public string? Paths { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}