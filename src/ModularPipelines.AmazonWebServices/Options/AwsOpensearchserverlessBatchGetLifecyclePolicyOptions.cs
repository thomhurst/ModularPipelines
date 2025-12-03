using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearchserverless", "batch-get-lifecycle-policy")]
public record AwsOpensearchserverlessBatchGetLifecyclePolicyOptions(
[property: CliOption("--identifiers")] string[] Identifiers
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}