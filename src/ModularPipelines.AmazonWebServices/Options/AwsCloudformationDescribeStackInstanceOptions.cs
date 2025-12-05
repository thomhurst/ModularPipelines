using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "describe-stack-instance")]
public record AwsCloudformationDescribeStackInstanceOptions(
[property: CliOption("--stack-set-name")] string StackSetName,
[property: CliOption("--stack-instance-account")] string StackInstanceAccount,
[property: CliOption("--stack-instance-region")] string StackInstanceRegion
) : AwsOptions
{
    [CliOption("--call-as")]
    public string? CallAs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}