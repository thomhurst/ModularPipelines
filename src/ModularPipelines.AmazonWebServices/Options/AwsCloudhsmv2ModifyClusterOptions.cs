using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudhsmv2", "modify-cluster")]
public record AwsCloudhsmv2ModifyClusterOptions(
[property: CommandSwitch("--backup-retention-policy")] string BackupRetentionPolicy,
[property: CommandSwitch("--cluster-id")] string ClusterId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}