using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("braket", "get-quantum-task")]
public record AwsBraketGetQuantumTaskOptions(
[property: CommandSwitch("--quantum-task-arn")] string QuantumTaskArn
) : AwsOptions
{
    [CommandSwitch("--additional-attribute-names")]
    public string[]? AdditionalAttributeNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}