using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-user-routing-profile")]
public record AwsConnectUpdateUserRoutingProfileOptions(
[property: CliOption("--routing-profile-id")] string RoutingProfileId,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}