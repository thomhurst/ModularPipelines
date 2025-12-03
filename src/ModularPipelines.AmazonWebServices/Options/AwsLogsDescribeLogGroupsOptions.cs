using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "describe-log-groups")]
public record AwsLogsDescribeLogGroupsOptions : AwsOptions
{
    [CliOption("--account-identifiers")]
    public string[]? AccountIdentifiers { get; set; }

    [CliOption("--log-group-name-prefix")]
    public string? LogGroupNamePrefix { get; set; }

    [CliOption("--log-group-name-pattern")]
    public string? LogGroupNamePattern { get; set; }

    [CliOption("--log-group-class")]
    public string? LogGroupClass { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}