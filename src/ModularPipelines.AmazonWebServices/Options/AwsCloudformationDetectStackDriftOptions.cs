using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "detect-stack-drift")]
public record AwsCloudformationDetectStackDriftOptions(
[property: CliOption("--stack-name")] string StackName
) : AwsOptions
{
    [CliOption("--logical-resource-ids")]
    public string[]? LogicalResourceIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}