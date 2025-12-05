using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("robomaker", "update-world-template")]
public record AwsRobomakerUpdateWorldTemplateOptions(
[property: CliOption("--template")] string Template
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--template-body")]
    public string? TemplateBody { get; set; }

    [CliOption("--template-location")]
    public string? TemplateLocation { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}