using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "delete-connection")]
public record AwsDmsDeleteConnectionOptions(
[property: CommandSwitch("--endpoint-arn")] string EndpointArn,
[property: CommandSwitch("--replication-instance-arn")] string ReplicationInstanceArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}