using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "associate-trial-component")]
public record AwsSagemakerAssociateTrialComponentOptions(
[property: CommandSwitch("--trial-component-name")] string TrialComponentName,
[property: CommandSwitch("--trial-name")] string TrialName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}