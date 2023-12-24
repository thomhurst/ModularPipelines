using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "delete-object")]
public record AwsClouddirectoryDeleteObjectOptions(
[property: CommandSwitch("--directory-arn")] string DirectoryArn,
[property: CommandSwitch("--object-reference")] string ObjectReference
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}