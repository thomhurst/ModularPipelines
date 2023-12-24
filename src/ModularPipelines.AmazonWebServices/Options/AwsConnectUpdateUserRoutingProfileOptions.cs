using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-user-routing-profile")]
public record AwsConnectUpdateUserRoutingProfileOptions(
[property: CommandSwitch("--routing-profile-id")] string RoutingProfileId,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}