using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune-graph", "create-graph")]
public record AwsNeptuneGraphCreateGraphOptions(
[property: CliOption("--graph-name")] string GraphName,
[property: CliOption("--provisioned-memory")] int ProvisionedMemory
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--kms-key-identifier")]
    public string? KmsKeyIdentifier { get; set; }

    [CliOption("--vector-search-configuration")]
    public string? VectorSearchConfiguration { get; set; }

    [CliOption("--replica-count")]
    public int? ReplicaCount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}