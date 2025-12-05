using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("scvmm", "vm", "nic", "list")]
public record AzScvmmVmNicListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions;