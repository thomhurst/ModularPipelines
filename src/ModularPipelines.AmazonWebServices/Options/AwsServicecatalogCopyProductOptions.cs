using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "copy-product")]
public record AwsServicecatalogCopyProductOptions(
[property: CliOption("--source-product-arn")] string SourceProductArn
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--target-product-id")]
    public string? TargetProductId { get; set; }

    [CliOption("--target-product-name")]
    public string? TargetProductName { get; set; }

    [CliOption("--source-provisioning-artifact-identifiers")]
    public string[]? SourceProvisioningArtifactIdentifiers { get; set; }

    [CliOption("--copy-options")]
    public string[]? CopyOptions { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}