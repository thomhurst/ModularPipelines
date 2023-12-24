using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "create-directory")]
public record AwsClouddirectoryCreateDirectoryOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--schema-arn")] string SchemaArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}