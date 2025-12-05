using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "delete-security-profile")]
public record AwsConnectDeleteSecurityProfileOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--security-profile-id")] string SecurityProfileId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}