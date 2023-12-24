using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-context")]
public record AwsSagemakerUpdateContextOptions(
[property: CommandSwitch("--context-name")] string ContextName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--properties")]
    public IEnumerable<KeyValue>? Properties { get; set; }

    [CommandSwitch("--properties-to-remove")]
    public string[]? PropertiesToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}