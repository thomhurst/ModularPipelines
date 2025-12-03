using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("translate", "import-terminology")]
public record AwsTranslateImportTerminologyOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--merge-strategy")] string MergeStrategy,
[property: CliOption("--data-file")] string DataFile
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--terminology-data")]
    public string? TerminologyData { get; set; }

    [CliOption("--encryption-key")]
    public string? EncryptionKey { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}