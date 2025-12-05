using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-user-identity-info")]
public record AwsConnectUpdateUserIdentityInfoOptions(
[property: CliOption("--identity-info")] string IdentityInfo,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}