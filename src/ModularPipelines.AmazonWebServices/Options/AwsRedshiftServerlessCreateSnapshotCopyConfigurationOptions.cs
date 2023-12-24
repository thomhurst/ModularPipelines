using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "create-snapshot-copy-configuration")]
public record AwsRedshiftServerlessCreateSnapshotCopyConfigurationOptions(
[property: CommandSwitch("--destination-region")] string DestinationRegion,
[property: CommandSwitch("--namespace-name")] string NamespaceName
) : AwsOptions
{
    [CommandSwitch("--destination-kms-key-id")]
    public string? DestinationKmsKeyId { get; set; }

    [CommandSwitch("--snapshot-retention-period")]
    public int? SnapshotRetentionPeriod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}