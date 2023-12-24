using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "wait", "run-completed")]
public record AwsOmicsWaitRunCompletedOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--export")]
    public string[]? Export { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}