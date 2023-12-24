using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "delete-db-instance-automated-backup")]
public record AwsRdsDeleteDbInstanceAutomatedBackupOptions : AwsOptions
{
    [CommandSwitch("--dbi-resource-id")]
    public string? DbiResourceId { get; set; }

    [CommandSwitch("--db-instance-automated-backups-arn")]
    public string? DbInstanceAutomatedBackupsArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}