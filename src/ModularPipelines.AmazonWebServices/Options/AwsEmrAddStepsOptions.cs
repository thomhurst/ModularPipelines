using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "add-steps")]
public record AwsEmrAddStepsOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--steps")] string[] Steps
) : AwsOptions
{
    [CommandSwitch("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }
}