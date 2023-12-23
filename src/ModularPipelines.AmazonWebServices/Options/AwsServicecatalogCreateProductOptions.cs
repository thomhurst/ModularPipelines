using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "create-product")]
public record AwsServicecatalogCreateProductOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--owner")] string Owner,
[property: CommandSwitch("--product-type")] string ProductType
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--distributor")]
    public string? Distributor { get; set; }

    [CommandSwitch("--support-description")]
    public string? SupportDescription { get; set; }

    [CommandSwitch("--support-email")]
    public string? SupportEmail { get; set; }

    [CommandSwitch("--support-url")]
    public string? SupportUrl { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--provisioning-artifact-parameters")]
    public string? ProvisioningArtifactParameters { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--source-connection")]
    public string? SourceConnection { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}