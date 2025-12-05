using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile", "update-project")]
public record AwsMobileUpdateProjectOptions(
[property: CliOption("--project-id")] string ProjectId
) : AwsOptions
{
    [CliOption("--contents")]
    public string? Contents { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}