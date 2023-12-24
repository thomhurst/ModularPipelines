using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "get-lifecycle-execution")]
public record AwsImagebuilderGetLifecycleExecutionOptions(
[property: CommandSwitch("--lifecycle-execution-id")] string LifecycleExecutionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}