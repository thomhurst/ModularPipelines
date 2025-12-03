using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "describe-stack-resource")]
public record AwsCloudformationDescribeStackResourceOptions(
[property: CliOption("--stack-name")] string StackName,
[property: CliOption("--logical-resource-id")] string LogicalResourceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}