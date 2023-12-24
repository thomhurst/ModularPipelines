using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune-graph", "create-graph")]
public record AwsNeptuneGraphCreateGraphOptions(
[property: CommandSwitch("--graph-name")] string GraphName,
[property: CommandSwitch("--provisioned-memory")] int ProvisionedMemory
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--kms-key-identifier")]
    public string? KmsKeyIdentifier { get; set; }

    [CommandSwitch("--vector-search-configuration")]
    public string? VectorSearchConfiguration { get; set; }

    [CommandSwitch("--replica-count")]
    public int? ReplicaCount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}