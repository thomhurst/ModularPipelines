using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune-graph", "create-graph-using-import-task")]
public record AwsNeptuneGraphCreateGraphUsingImportTaskOptions(
[property: CommandSwitch("--graph-name")] string GraphName,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--role-arn")] string RoleArn
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

    [CommandSwitch("--import-options")]
    public string? ImportOptions { get; set; }

    [CommandSwitch("--max-provisioned-memory")]
    public int? MaxProvisionedMemory { get; set; }

    [CommandSwitch("--min-provisioned-memory")]
    public int? MinProvisionedMemory { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}