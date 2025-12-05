using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "create-product")]
public record AwsServicecatalogCreateProductOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--owner")] string Owner,
[property: CliOption("--product-type")] string ProductType
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--distributor")]
    public string? Distributor { get; set; }

    [CliOption("--support-description")]
    public string? SupportDescription { get; set; }

    [CliOption("--support-email")]
    public string? SupportEmail { get; set; }

    [CliOption("--support-url")]
    public string? SupportUrl { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--provisioning-artifact-parameters")]
    public string? ProvisioningArtifactParameters { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--source-connection")]
    public string? SourceConnection { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}