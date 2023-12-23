using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qldb", "describe-journal-s3-export")]
public record AwsQldbDescribeJournalS3ExportOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--export-id")] string ExportId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}