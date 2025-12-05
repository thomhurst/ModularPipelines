using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "detect-stack-resource-drift")]
public record AwsCloudformationDetectStackResourceDriftOptions(
[property: CliOption("--stack-name")] string StackName,
[property: CliOption("--logical-resource-id")] string LogicalResourceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}