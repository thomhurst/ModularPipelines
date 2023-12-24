using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "classify-document")]
public record AwsComprehendClassifyDocumentOptions(
[property: CommandSwitch("--endpoint-arn")] string EndpointArn
) : AwsOptions
{
    [CommandSwitch("--text")]
    public string? Text { get; set; }

    [CommandSwitch("--bytes")]
    public string? Bytes { get; set; }

    [CommandSwitch("--document-reader-config")]
    public string? DocumentReaderConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}