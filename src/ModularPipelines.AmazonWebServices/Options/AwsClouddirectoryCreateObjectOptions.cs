using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "create-object")]
public record AwsClouddirectoryCreateObjectOptions(
[property: CommandSwitch("--directory-arn")] string DirectoryArn,
[property: CommandSwitch("--schema-facets")] string[] SchemaFacets
) : AwsOptions
{
    [CommandSwitch("--object-attribute-list")]
    public string[]? ObjectAttributeList { get; set; }

    [CommandSwitch("--parent-reference")]
    public string? ParentReference { get; set; }

    [CommandSwitch("--link-name")]
    public string? LinkName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}