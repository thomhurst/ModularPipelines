using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "replicate-instance")]
public record AwsConnectReplicateInstanceOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--replica-region")] string ReplicaRegion,
[property: CliOption("--replica-alias")] string ReplicaAlias
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}