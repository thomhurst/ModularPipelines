using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "replicate-instance")]
public record AwsConnectReplicateInstanceOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--replica-region")] string ReplicaRegion,
[property: CommandSwitch("--replica-alias")] string ReplicaAlias
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}