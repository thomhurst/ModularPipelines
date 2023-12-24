using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "list-executions")]
public record AwsStepfunctionsListExecutionsOptions : AwsOptions
{
    [CommandSwitch("--state-machine-arn")]
    public string? StateMachineArn { get; set; }

    [CommandSwitch("--status-filter")]
    public string? StatusFilter { get; set; }

    [CommandSwitch("--map-run-arn")]
    public string? MapRunArn { get; set; }

    [CommandSwitch("--redrive-filter")]
    public string? RedriveFilter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}