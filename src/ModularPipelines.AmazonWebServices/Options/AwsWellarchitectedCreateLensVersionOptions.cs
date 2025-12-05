using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "create-lens-version")]
public record AwsWellarchitectedCreateLensVersionOptions(
[property: CliOption("--lens-alias")] string LensAlias,
[property: CliOption("--lens-version")] string LensVersion
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}