using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("robomaker", "get-world-template-body")]
public record AwsRobomakerGetWorldTemplateBodyOptions : AwsOptions
{
    [CliOption("--template")]
    public string? Template { get; set; }

    [CliOption("--generation-job")]
    public string? GenerationJob { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}