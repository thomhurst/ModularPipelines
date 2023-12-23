using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "attach-typed-link")]
public record AwsClouddirectoryAttachTypedLinkOptions(
[property: CommandSwitch("--directory-arn")] string DirectoryArn,
[property: CommandSwitch("--source-object-reference")] string SourceObjectReference,
[property: CommandSwitch("--target-object-reference")] string TargetObjectReference,
[property: CommandSwitch("--typed-link-facet")] string TypedLinkFacet,
[property: CommandSwitch("--attributes")] string[] Attributes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}