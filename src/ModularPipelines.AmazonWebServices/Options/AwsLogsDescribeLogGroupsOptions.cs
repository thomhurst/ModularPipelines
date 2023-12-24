using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "describe-log-groups")]
public record AwsLogsDescribeLogGroupsOptions : AwsOptions
{
    [CommandSwitch("--account-identifiers")]
    public string[]? AccountIdentifiers { get; set; }

    [CommandSwitch("--log-group-name-prefix")]
    public string? LogGroupNamePrefix { get; set; }

    [CommandSwitch("--log-group-name-pattern")]
    public string? LogGroupNamePattern { get; set; }

    [CommandSwitch("--log-group-class")]
    public string? LogGroupClass { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}