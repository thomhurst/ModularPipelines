using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "wait", "run-completed")]
public record AwsOmicsWaitRunCompletedOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--export")]
    public string[]? Export { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}