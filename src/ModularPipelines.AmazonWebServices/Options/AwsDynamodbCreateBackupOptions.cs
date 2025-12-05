using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "create-backup")]
public record AwsDynamodbCreateBackupOptions(
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--backup-name")] string BackupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}