using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "get-lens-version-difference")]
public record AwsWellarchitectedGetLensVersionDifferenceOptions(
[property: CliOption("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CliOption("--base-lens-version")]
    public string? BaseLensVersion { get; set; }

    [CliOption("--target-lens-version")]
    public string? TargetLensVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}