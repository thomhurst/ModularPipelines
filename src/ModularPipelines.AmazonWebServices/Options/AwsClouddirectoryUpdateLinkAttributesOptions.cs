using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "update-link-attributes")]
public record AwsClouddirectoryUpdateLinkAttributesOptions(
[property: CommandSwitch("--directory-arn")] string DirectoryArn,
[property: CommandSwitch("--typed-link-specifier")] string TypedLinkSpecifier,
[property: CommandSwitch("--attribute-updates")] string[] AttributeUpdates
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}