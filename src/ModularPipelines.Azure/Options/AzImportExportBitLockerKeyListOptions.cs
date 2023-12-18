using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("import-export", "bit-locker-key", "list")]
public record AzImportExportBitLockerKeyListOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;