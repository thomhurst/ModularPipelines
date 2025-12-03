using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearchserverless", "batch-get-vpc-endpoint")]
public record AwsOpensearchserverlessBatchGetVpcEndpointOptions(
[property: CliOption("--ids")] string[] Ids
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}