using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "disassociate-trial-component")]
public record AwsSagemakerDisassociateTrialComponentOptions(
[property: CliOption("--trial-component-name")] string TrialComponentName,
[property: CliOption("--trial-name")] string TrialName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}