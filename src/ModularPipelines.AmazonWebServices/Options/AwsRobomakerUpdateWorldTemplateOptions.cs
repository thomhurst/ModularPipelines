using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "update-world-template")]
public record AwsRobomakerUpdateWorldTemplateOptions(
[property: CommandSwitch("--template")] string Template
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--template-body")]
    public string? TemplateBody { get; set; }

    [CommandSwitch("--template-location")]
    public string? TemplateLocation { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}