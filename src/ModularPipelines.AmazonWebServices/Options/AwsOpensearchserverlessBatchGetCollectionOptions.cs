using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearchserverless", "batch-get-collection")]
public record AwsOpensearchserverlessBatchGetCollectionOptions : AwsOptions
{
    [CliOption("--ids")]
    public string[]? Ids { get; set; }

    [CliOption("--names")]
    public string[]? Names { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}