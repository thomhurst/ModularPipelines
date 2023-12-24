using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "get-lens-version-difference")]
public record AwsWellarchitectedGetLensVersionDifferenceOptions(
[property: CommandSwitch("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CommandSwitch("--base-lens-version")]
    public string? BaseLensVersion { get; set; }

    [CommandSwitch("--target-lens-version")]
    public string? TargetLensVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}