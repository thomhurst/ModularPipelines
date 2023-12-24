using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "copy-product")]
public record AwsServicecatalogCopyProductOptions(
[property: CommandSwitch("--source-product-arn")] string SourceProductArn
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--target-product-id")]
    public string? TargetProductId { get; set; }

    [CommandSwitch("--target-product-name")]
    public string? TargetProductName { get; set; }

    [CommandSwitch("--source-provisioning-artifact-identifiers")]
    public string[]? SourceProvisioningArtifactIdentifiers { get; set; }

    [CommandSwitch("--copy-options")]
    public string[]? CopyOptions { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}