using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "delete-db-cluster-automated-backup")]
public record AwsRdsDeleteDbClusterAutomatedBackupOptions(
[property: CliOption("--db-cluster-resource-id")] string DbClusterResourceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}