using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "wait", "type-registration-complete")]
public record AwsCloudformationWaitTypeRegistrationCompleteOptions(
[property: CommandSwitch("--registration-token")] string RegistrationToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}