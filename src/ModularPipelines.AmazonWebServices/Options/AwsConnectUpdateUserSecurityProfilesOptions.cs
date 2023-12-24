using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-user-security-profiles")]
public record AwsConnectUpdateUserSecurityProfilesOptions(
[property: CommandSwitch("--security-profile-ids")] string[] SecurityProfileIds,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}