using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "list-schedules")]
public record AwsSchedulerListSchedulesOptions : AwsOptions
{
    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--name-prefix")]
    public string? NamePrefix { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}