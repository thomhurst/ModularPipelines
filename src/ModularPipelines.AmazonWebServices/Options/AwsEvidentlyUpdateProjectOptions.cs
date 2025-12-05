using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "update-project")]
public record AwsEvidentlyUpdateProjectOptions(
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--app-config-resource")]
    public string? AppConfigResource { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}