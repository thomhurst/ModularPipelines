using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "list-provisioning-artifacts-for-service-action")]
public record AwsServicecatalogListProvisioningArtifactsForServiceActionOptions(
[property: CommandSwitch("--service-action-id")] string ServiceActionId
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