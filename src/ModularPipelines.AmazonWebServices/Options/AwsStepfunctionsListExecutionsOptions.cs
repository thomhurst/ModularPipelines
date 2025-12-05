using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "list-executions")]
public record AwsStepfunctionsListExecutionsOptions : AwsOptions
{
    [CliOption("--state-machine-arn")]
    public string? StateMachineArn { get; set; }

    [CliOption("--status-filter")]
    public string? StatusFilter { get; set; }

    [CliOption("--map-run-arn")]
    public string? MapRunArn { get; set; }

    [CliOption("--redrive-filter")]
    public string? RedriveFilter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}