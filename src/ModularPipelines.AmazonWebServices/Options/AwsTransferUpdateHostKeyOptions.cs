using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "update-host-key")]
public record AwsTransferUpdateHostKeyOptions(
[property: CommandSwitch("--server-id")] string ServerId,
[property: CommandSwitch("--host-key-id")] string HostKeyId,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}