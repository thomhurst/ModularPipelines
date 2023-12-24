using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "associate-security-key")]
public record AwsConnectAssociateSecurityKeyOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--key")] string Key
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}