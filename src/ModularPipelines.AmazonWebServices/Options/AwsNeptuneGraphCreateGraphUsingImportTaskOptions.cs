using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune-graph", "create-graph-using-import-task")]
public record AwsNeptuneGraphCreateGraphUsingImportTaskOptions(
[property: CliOption("--graph-name")] string GraphName,
[property: CliOption("--source")] string Source,
[property: CliOption("--role-arn")] string RoleArn
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

    [CliOption("--import-options")]
    public string? ImportOptions { get; set; }

    [CliOption("--max-provisioned-memory")]
    public int? MaxProvisionedMemory { get; set; }

    [CliOption("--min-provisioned-memory")]
    public int? MinProvisionedMemory { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}