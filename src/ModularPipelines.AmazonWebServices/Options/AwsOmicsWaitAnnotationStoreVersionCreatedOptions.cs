using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "wait", "annotation-store-version-created")]
public record AwsOmicsWaitAnnotationStoreVersionCreatedOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--version-name")] string VersionName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}