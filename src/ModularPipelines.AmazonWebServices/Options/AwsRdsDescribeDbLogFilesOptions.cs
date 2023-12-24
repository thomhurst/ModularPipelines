using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "describe-db-log-files")]
public record AwsRdsDescribeDbLogFilesOptions(
[property: CommandSwitch("--db-instance-identifier")] string DbInstanceIdentifier
) : AwsOptions
{
    [CommandSwitch("--filename-contains")]
    public string? FilenameContains { get; set; }

    [CommandSwitch("--file-last-written")]
    public long? FileLastWritten { get; set; }

    [CommandSwitch("--file-size")]
    public long? FileSize { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}