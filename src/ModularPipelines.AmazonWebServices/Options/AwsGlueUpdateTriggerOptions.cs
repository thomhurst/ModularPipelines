using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "update-trigger")]
public record AwsGlueUpdateTriggerOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--trigger-update")] string TriggerUpdate
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}