using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "put-organization-conformance-pack")]
public record AwsConfigservicePutOrganizationConformancePackOptions(
[property: CliOption("--organization-conformance-pack-name")] string OrganizationConformancePackName
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

    [CliOption("--excluded-accounts")]
    public string[]? ExcludedAccounts { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}