using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "add-facet-to-object")]
public record AwsClouddirectoryAddFacetToObjectOptions(
[property: CommandSwitch("--directory-arn")] string DirectoryArn,
[property: CommandSwitch("--schema-facet")] string SchemaFacet,
[property: CommandSwitch("--object-reference")] string ObjectReference
) : AwsOptions
{
    [CommandSwitch("--object-attribute-list")]
    public string[]? ObjectAttributeList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}