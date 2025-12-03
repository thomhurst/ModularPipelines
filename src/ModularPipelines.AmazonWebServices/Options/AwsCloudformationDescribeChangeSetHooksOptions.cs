using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "describe-change-set-hooks")]
public record AwsCloudformationDescribeChangeSetHooksOptions(
[property: CliOption("--change-set-name")] string ChangeSetName
) : AwsOptions
{
    [CliOption("--stack-name")]
    public string? StackName { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--logical-resource-id")]
    public string? LogicalResourceId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}