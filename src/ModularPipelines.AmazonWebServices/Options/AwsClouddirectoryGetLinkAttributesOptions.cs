using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "get-link-attributes")]
public record AwsClouddirectoryGetLinkAttributesOptions(
[property: CommandSwitch("--directory-arn")] string DirectoryArn,
[property: CommandSwitch("--typed-link-specifier")] string TypedLinkSpecifier,
[property: CommandSwitch("--attribute-names")] string[] AttributeNames
) : AwsOptions
{
    [CommandSwitch("--consistency-level")]
    public string? ConsistencyLevel { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}