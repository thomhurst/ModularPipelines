using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "delete-model")]
public record AwsFrauddetectorDeleteModelOptions(
[property: CommandSwitch("--model-id")] string ModelId,
[property: CommandSwitch("--model-type")] string ModelType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}