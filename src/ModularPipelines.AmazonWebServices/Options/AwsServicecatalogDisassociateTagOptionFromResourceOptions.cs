using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "disassociate-tag-option-from-resource")]
public record AwsServicecatalogDisassociateTagOptionFromResourceOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--tag-option-id")] string TagOptionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}