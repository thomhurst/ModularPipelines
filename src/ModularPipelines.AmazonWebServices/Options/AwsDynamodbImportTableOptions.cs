using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "import-table")]
public record AwsDynamodbImportTableOptions(
[property: CliOption("--s3-bucket-source")] string S3BucketSource,
[property: CliOption("--input-format")] string InputFormat,
[property: CliOption("--table-creation-parameters")] string TableCreationParameters
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--input-format-options")]
    public string? InputFormatOptions { get; set; }

    [CliOption("--input-compression-type")]
    public string? InputCompressionType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}