using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "get-world-template-body")]
public record AwsRobomakerGetWorldTemplateBodyOptions : AwsOptions
{
    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [CommandSwitch("--generation-job")]
    public string? GenerationJob { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}