using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "disassociate-mac-sec-key")]
public record AwsDirectconnectDisassociateMacSecKeyOptions(
[property: CommandSwitch("--connection-id")] string ConnectionId,
[property: CommandSwitch("--secret-arn")] string SecretArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}