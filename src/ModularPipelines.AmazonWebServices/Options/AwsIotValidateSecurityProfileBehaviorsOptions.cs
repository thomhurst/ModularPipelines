using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "validate-security-profile-behaviors")]
public record AwsIotValidateSecurityProfileBehaviorsOptions(
[property: CommandSwitch("--behaviors")] string[] Behaviors
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}