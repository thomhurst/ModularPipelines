using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "put-conformance-pack")]
public record AwsConfigservicePutConformancePackOptions(
[property: CommandSwitch("--conformance-pack-name")] string ConformancePackName
) : AwsOptions
{
    [CommandSwitch("--template-s3-uri")]
    public string? TemplateS3Uri { get; set; }

    [CommandSwitch("--template-body")]
    public string? TemplateBody { get; set; }

    [CommandSwitch("--delivery-s3-bucket")]
    public string? DeliveryS3Bucket { get; set; }

    [CommandSwitch("--delivery-s3-key-prefix")]
    public string? DeliveryS3KeyPrefix { get; set; }

    [CommandSwitch("--conformance-pack-input-parameters")]
    public string[]? ConformancePackInputParameters { get; set; }

    [CommandSwitch("--template-ssm-document-details")]
    public string? TemplateSsmDocumentDetails { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}