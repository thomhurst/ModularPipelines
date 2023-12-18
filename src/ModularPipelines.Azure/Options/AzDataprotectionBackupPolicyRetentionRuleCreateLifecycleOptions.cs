using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-policy", "retention-rule", "create-lifecycle")]
public record AzDataprotectionBackupPolicyRetentionRuleCreateLifecycleOptions(
[property: CommandSwitch("--count")] int Count,
[property: CommandSwitch("--retention-duration-type")] string RetentionDurationType,
[property: CommandSwitch("--source-datastore")] string SourceDatastore
) : AzOptions
{
    [CommandSwitch("--copy-option")]
    public string? CopyOption { get; set; }

    [CommandSwitch("--target-datastore")]
    public string? TargetDatastore { get; set; }
}