using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "put-conformance-pack")]
public record AwsConfigservicePutConformancePackOptions(
[property: CliOption("--conformance-pack-name")] string ConformancePackName
) : AwsOptions
{
    [CliOption("--template-s3-uri")]
    public string? TemplateS3Uri { get; set; }

    [CliOption("--template-body")]
    public string? TemplateBody { get; set; }

    [CliOption("--delivery-s3-bucket")]
    public string? DeliveryS3Bucket { get; set; }

    [CliOption("--delivery-s3-key-prefix")]
    public string? DeliveryS3KeyPrefix { get; set; }

    [CliOption("--conformance-pack-input-parameters")]
    public string[]? ConformancePackInputParameters { get; set; }

    [CliOption("--template-ssm-document-details")]
    public string? TemplateSsmDocumentDetails { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}