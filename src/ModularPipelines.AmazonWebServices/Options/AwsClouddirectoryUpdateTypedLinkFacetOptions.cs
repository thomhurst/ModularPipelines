using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "update-typed-link-facet")]
public record AwsClouddirectoryUpdateTypedLinkFacetOptions(
[property: CommandSwitch("--schema-arn")] string SchemaArn,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--attribute-updates")] string[] AttributeUpdates,
[property: CommandSwitch("--identity-attribute-order")] string[] IdentityAttributeOrder
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}