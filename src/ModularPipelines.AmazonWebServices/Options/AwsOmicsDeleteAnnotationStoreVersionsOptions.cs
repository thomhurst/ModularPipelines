using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "delete-annotation-store-versions")]
public record AwsOmicsDeleteAnnotationStoreVersionsOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--versions")] string[] Versions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}