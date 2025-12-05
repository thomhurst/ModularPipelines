using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "update-flywheel")]
public record AwsComprehendUpdateFlywheelOptions(
[property: CliOption("--flywheel-arn")] string FlywheelArn
) : AwsOptions
{
    [CliOption("--active-model-arn")]
    public string? ActiveModelArn { get; set; }

    [CliOption("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CliOption("--data-security-config")]
    public string? DataSecurityConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}