using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "update-project")]
public record AwsEvidentlyUpdateProjectOptions(
[property: CommandSwitch("--project")] string Project
) : AwsOptions
{
    [CommandSwitch("--app-config-resource")]
    public string? AppConfigResource { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}