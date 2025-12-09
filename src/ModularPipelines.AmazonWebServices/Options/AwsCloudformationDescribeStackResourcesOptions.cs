using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "describe-stack-resources")]
public record AwsCloudformationDescribeStackResourcesOptions : AwsOptions
{
    [CliOption("--stack-name")]
    public string? StackName { get; set; }

    [CliOption("--logical-resource-id")]
    public string? LogicalResourceId { get; set; }

    [CliOption("--physical-resource-id")]
    public string? PhysicalResourceId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}