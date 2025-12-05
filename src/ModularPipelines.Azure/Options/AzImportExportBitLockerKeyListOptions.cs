using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("import-export", "bit-locker-key", "list")]
public record AzImportExportBitLockerKeyListOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;