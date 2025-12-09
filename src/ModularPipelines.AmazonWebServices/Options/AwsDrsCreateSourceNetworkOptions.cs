using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("drs", "create-source-network")]
public record AwsDrsCreateSourceNetworkOptions(
[property: CliOption("--origin-account-id")] string OriginAccountId,
[property: CliOption("--origin-region")] string OriginRegion,
[property: CliOption("--vpc-id")] string VpcId
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}