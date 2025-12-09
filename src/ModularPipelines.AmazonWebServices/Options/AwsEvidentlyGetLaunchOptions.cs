using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "get-launch")]
public record AwsEvidentlyGetLaunchOptions(
[property: CliOption("--launch")] string Launch,
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}