using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "get-lens")]
public record AwsWellarchitectedGetLensOptions(
[property: CliOption("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CliOption("--lens-version")]
    public string? LensVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}