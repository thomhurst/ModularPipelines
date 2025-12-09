using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudhsmv2", "modify-cluster")]
public record AwsCloudhsmv2ModifyClusterOptions(
[property: CliOption("--backup-retention-policy")] string BackupRetentionPolicy,
[property: CliOption("--cluster-id")] string ClusterId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}