using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "delete-namespace")]
public record AwsRedshiftServerlessDeleteNamespaceOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName
) : AwsOptions
{
    [CommandSwitch("--final-snapshot-name")]
    public string? FinalSnapshotName { get; set; }

    [CommandSwitch("--final-snapshot-retention-period")]
    public int? FinalSnapshotRetentionPeriod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}