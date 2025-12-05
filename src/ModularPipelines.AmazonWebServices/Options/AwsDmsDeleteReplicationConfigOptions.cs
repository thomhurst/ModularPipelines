using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "delete-replication-config")]
public record AwsDmsDeleteReplicationConfigOptions(
[property: CliOption("--replication-config-arn")] string ReplicationConfigArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}