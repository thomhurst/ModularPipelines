using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "list-service-actions-for-provisioning-artifact")]
public record AwsServicecatalogListServiceActionsForProvisioningArtifactOptions(
[property: CommandSwitch("--product-id")] string ProductId,
[property: CommandSwitch("--provisioning-artifact-id")] string ProvisioningArtifactId
) : AwsOptions
{
    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}