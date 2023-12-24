using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "detach-typed-link")]
public record AwsClouddirectoryDetachTypedLinkOptions(
[property: CommandSwitch("--directory-arn")] string DirectoryArn,
[property: CommandSwitch("--typed-link-specifier")] string TypedLinkSpecifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}