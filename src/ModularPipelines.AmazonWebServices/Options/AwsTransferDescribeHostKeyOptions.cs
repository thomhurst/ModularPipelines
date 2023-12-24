using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "describe-host-key")]
public record AwsTransferDescribeHostKeyOptions(
[property: CommandSwitch("--server-id")] string ServerId,
[property: CommandSwitch("--host-key-id")] string HostKeyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}