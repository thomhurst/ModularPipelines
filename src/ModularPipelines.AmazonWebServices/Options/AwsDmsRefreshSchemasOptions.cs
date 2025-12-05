using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "refresh-schemas")]
public record AwsDmsRefreshSchemasOptions(
[property: CliOption("--endpoint-arn")] string EndpointArn,
[property: CliOption("--replication-instance-arn")] string ReplicationInstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}