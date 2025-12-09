using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "get-run")]
public record AwsOmicsGetRunOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--export")]
    public string[]? Export { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}