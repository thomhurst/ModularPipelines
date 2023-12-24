using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "create-model")]
public record AwsFrauddetectorCreateModelOptions(
[property: CommandSwitch("--model-id")] string ModelId,
[property: CommandSwitch("--model-type")] string ModelType,
[property: CommandSwitch("--event-type-name")] string EventTypeName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}