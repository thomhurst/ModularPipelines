using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("braket", "get-quantum-task")]
public record AwsBraketGetQuantumTaskOptions(
[property: CliOption("--quantum-task-arn")] string QuantumTaskArn
) : AwsOptions
{
    [CliOption("--additional-attribute-names")]
    public string[]? AdditionalAttributeNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}