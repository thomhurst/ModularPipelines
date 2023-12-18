using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-policy", "tag", "remove")]
public record AzDataprotectionBackupPolicyTagRemoveOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy")] string Policy
) : AzOptions;