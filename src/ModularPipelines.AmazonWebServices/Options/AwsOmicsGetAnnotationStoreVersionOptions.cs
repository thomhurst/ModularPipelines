using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "get-annotation-store-version")]
public record AwsOmicsGetAnnotationStoreVersionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--version-name")] string VersionName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}