using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-trial")]
public record AwsSagemakerUpdateTrialOptions(
[property: CliOption("--trial-name")] string TrialName
) : AwsOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}