using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "describe-db-instance-automated-backups")]
public record AwsRdsDescribeDbInstanceAutomatedBackupsOptions : AwsOptions
{
    [CommandSwitch("--dbi-resource-id")]
    public string? DbiResourceId { get; set; }

    [CommandSwitch("--db-instance-identifier")]
    public string? DbInstanceIdentifier { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--db-instance-automated-backups-arn")]
    public string? DbInstanceAutomatedBackupsArn { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}