using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "delete-db-cluster-automated-backup")]
public record AwsRdsDeleteDbClusterAutomatedBackupOptions(
[property: CommandSwitch("--db-cluster-resource-id")] string DbClusterResourceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}