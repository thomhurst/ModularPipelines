using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qldb", "export-journal-to-s3")]
public record AwsQldbExportJournalToS3Options(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--inclusive-start-time")] long InclusiveStartTime,
[property: CommandSwitch("--exclusive-end-time")] long ExclusiveEndTime,
[property: CommandSwitch("--s3-export-configuration")] string S3ExportConfiguration,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--output-format")]
    public string? OutputFormat { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}