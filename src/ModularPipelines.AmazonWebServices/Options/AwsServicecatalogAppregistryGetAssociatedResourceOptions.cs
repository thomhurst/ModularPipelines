using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog-appregistry", "get-associated-resource")]
public record AwsServicecatalogAppregistryGetAssociatedResourceOptions(
[property: CommandSwitch("--application")] string Application,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--resource")] string Resource
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--resource-tag-status")]
    public string[]? ResourceTagStatus { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}