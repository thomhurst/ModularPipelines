using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "describe-log-streams")]
public record AwsLogsDescribeLogStreamsOptions : AwsOptions
{
    [CliOption("--log-group-name")]
    public string? LogGroupName { get; set; }

    [CliOption("--log-group-identifier")]
    public string? LogGroupIdentifier { get; set; }

    [CliOption("--log-stream-name-prefix")]
    public string? LogStreamNamePrefix { get; set; }

    [CliOption("--order-by")]
    public string? OrderBy { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}