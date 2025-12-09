using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "update-project")]
public record AwsIotsitewiseUpdateProjectOptions(
[property: CliOption("--project-id")] string ProjectId,
[property: CliOption("--project-name")] string ProjectName
) : AwsOptions
{
    [CliOption("--project-description")]
    public string? ProjectDescription { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}