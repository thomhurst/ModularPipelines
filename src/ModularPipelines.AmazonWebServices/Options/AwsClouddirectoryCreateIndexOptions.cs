using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "create-index")]
public record AwsClouddirectoryCreateIndexOptions(
[property: CommandSwitch("--directory-arn")] string DirectoryArn,
[property: CommandSwitch("--ordered-indexed-attribute-list")] string[] OrderedIndexedAttributeList
) : AwsOptions
{
    [CommandSwitch("--parent-reference")]
    public string? ParentReference { get; set; }

    [CommandSwitch("--link-name")]
    public string? LinkName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}