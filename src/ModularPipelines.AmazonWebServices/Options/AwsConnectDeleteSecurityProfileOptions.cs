using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "delete-security-profile")]
public record AwsConnectDeleteSecurityProfileOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--security-profile-id")] string SecurityProfileId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}