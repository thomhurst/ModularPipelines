using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "import-table")]
public record AwsDynamodbImportTableOptions(
[property: CommandSwitch("--s3-bucket-source")] string S3BucketSource,
[property: CommandSwitch("--input-format")] string InputFormat,
[property: CommandSwitch("--table-creation-parameters")] string TableCreationParameters
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--input-format-options")]
    public string? InputFormatOptions { get; set; }

    [CommandSwitch("--input-compression-type")]
    public string? InputCompressionType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}