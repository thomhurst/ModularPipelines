using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "classify-document")]
public record AwsComprehendClassifyDocumentOptions(
[property: CliOption("--endpoint-arn")] string EndpointArn
) : AwsOptions
{
    [CliOption("--text")]
    public string? Text { get; set; }

    [CliOption("--bytes")]
    public string? Bytes { get; set; }

    [CliOption("--document-reader-config")]
    public string? DocumentReaderConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}