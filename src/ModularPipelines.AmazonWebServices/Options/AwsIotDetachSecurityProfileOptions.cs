using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "detach-security-profile")]
public record AwsIotDetachSecurityProfileOptions(
[property: CliOption("--security-profile-name")] string SecurityProfileName,
[property: CliOption("--security-profile-target-arn")] string SecurityProfileTargetArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}