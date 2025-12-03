using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "validate-security-profile-behaviors")]
public record AwsIotValidateSecurityProfileBehaviorsOptions(
[property: CliOption("--behaviors")] string[] Behaviors
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}