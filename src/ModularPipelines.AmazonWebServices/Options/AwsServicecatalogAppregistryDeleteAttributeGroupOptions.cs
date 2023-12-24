using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog-appregistry", "delete-attribute-group")]
public record AwsServicecatalogAppregistryDeleteAttributeGroupOptions(
[property: CommandSwitch("--attribute-group")] string AttributeGroup
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}