using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "update-flywheel")]
public record AwsComprehendUpdateFlywheelOptions(
[property: CommandSwitch("--flywheel-arn")] string FlywheelArn
) : AwsOptions
{
    [CommandSwitch("--active-model-arn")]
    public string? ActiveModelArn { get; set; }

    [CommandSwitch("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CommandSwitch("--data-security-config")]
    public string? DataSecurityConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}