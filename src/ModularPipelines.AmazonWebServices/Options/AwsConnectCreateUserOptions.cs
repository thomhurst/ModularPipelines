using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "create-user")]
public record AwsConnectCreateUserOptions(
[property: CommandSwitch("--username")] string Username,
[property: CommandSwitch("--phone-config")] string PhoneConfig,
[property: CommandSwitch("--security-profile-ids")] string[] SecurityProfileIds,
[property: CommandSwitch("--routing-profile-id")] string RoutingProfileId,
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--identity-info")]
    public string? IdentityInfo { get; set; }

    [CommandSwitch("--directory-user-id")]
    public string? DirectoryUserId { get; set; }

    [CommandSwitch("--hierarchy-group-id")]
    public string? HierarchyGroupId { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}