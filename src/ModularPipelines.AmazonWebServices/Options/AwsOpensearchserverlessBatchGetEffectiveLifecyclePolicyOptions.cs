using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearchserverless", "batch-get-effective-lifecycle-policy")]
public record AwsOpensearchserverlessBatchGetEffectiveLifecyclePolicyOptions(
[property: CliOption("--resource-identifiers")] string[] ResourceIdentifiers
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}