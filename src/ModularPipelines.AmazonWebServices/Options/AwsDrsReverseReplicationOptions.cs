using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("drs", "reverse-replication")]
public record AwsDrsReverseReplicationOptions(
[property: CliOption("--recovery-instance-id")] string RecoveryInstanceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}