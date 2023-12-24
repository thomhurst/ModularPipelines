using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "create-backup")]
public record AwsDynamodbCreateBackupOptions(
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--backup-name")] string BackupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}