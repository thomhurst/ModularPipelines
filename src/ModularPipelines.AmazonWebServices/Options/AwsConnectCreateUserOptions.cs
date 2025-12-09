using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-user")]
public record AwsConnectCreateUserOptions(
[property: CliOption("--username")] string Username,
[property: CliOption("--phone-config")] string PhoneConfig,
[property: CliOption("--security-profile-ids")] string[] SecurityProfileIds,
[property: CliOption("--routing-profile-id")] string RoutingProfileId,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--identity-info")]
    public string? IdentityInfo { get; set; }

    [CliOption("--directory-user-id")]
    public string? DirectoryUserId { get; set; }

    [CliOption("--hierarchy-group-id")]
    public string? HierarchyGroupId { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}