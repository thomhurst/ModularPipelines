using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "detect-entities")]
public record AwsComprehendDetectEntitiesOptions : AwsOptions
{
    [CliOption("--text")]
    public string? Text { get; set; }

    [CliOption("--language-code")]
    public string? LanguageCode { get; set; }

    [CliOption("--endpoint-arn")]
    public string? EndpointArn { get; set; }

    [CliOption("--bytes")]
    public string? Bytes { get; set; }

    [CliOption("--document-reader-config")]
    public string? DocumentReaderConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}