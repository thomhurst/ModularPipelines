using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "test-connection")]
public record AwsDmsTestConnectionOptions(
[property: CliOption("--replication-instance-arn")] string ReplicationInstanceArn,
[property: CliOption("--endpoint-arn")] string EndpointArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}