using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataprotection", "backup-policy", "retention-rule", "create-lifecycle")]
public record AzDataprotectionBackupPolicyRetentionRuleCreateLifecycleOptions(
[property: CliOption("--count")] int Count,
[property: CliOption("--retention-duration-type")] string RetentionDurationType,
[property: CliOption("--source-datastore")] string SourceDatastore
) : AzOptions
{
    [CliOption("--copy-option")]
    public string? CopyOption { get; set; }

    [CliOption("--target-datastore")]
    public string? TargetDatastore { get; set; }
}