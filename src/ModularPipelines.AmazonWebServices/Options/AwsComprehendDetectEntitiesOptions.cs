using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "detect-entities")]
public record AwsComprehendDetectEntitiesOptions : AwsOptions
{
    [CommandSwitch("--text")]
    public string? Text { get; set; }

    [CommandSwitch("--language-code")]
    public string? LanguageCode { get; set; }

    [CommandSwitch("--endpoint-arn")]
    public string? EndpointArn { get; set; }

    [CommandSwitch("--bytes")]
    public string? Bytes { get; set; }

    [CommandSwitch("--document-reader-config")]
    public string? DocumentReaderConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}