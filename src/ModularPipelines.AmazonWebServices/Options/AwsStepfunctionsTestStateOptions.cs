using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "test-state")]
public record AwsStepfunctionsTestStateOptions(
[property: CommandSwitch("--definition")] string Definition,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--input")]
    public string? Input { get; set; }

    [CommandSwitch("--inspection-level")]
    public string? InspectionLevel { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}