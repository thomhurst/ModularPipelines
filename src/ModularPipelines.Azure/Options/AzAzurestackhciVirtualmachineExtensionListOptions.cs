using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("urestackhci", "virtualmachine", "extension", "list")]
public record AzAzurestackhciVirtualmachineExtensionListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--virtualmachine-name")] string VirtualmachineName
) : AzOptions;