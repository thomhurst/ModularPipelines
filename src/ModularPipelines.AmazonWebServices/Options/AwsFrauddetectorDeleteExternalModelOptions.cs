using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "delete-external-model")]
public record AwsFrauddetectorDeleteExternalModelOptions(
[property: CommandSwitch("--model-endpoint")] string ModelEndpoint
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}