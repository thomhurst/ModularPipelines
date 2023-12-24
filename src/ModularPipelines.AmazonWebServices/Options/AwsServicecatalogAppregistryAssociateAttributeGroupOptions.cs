using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog-appregistry", "associate-attribute-group")]
public record AwsServicecatalogAppregistryAssociateAttributeGroupOptions(
[property: CommandSwitch("--application")] string Application,
[property: CommandSwitch("--attribute-group")] string AttributeGroup
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}