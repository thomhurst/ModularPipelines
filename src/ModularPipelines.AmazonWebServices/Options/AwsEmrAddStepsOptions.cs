using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "add-steps")]
public record AwsEmrAddStepsOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--steps")] string[] Steps
) : AwsOptions
{
    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }
}