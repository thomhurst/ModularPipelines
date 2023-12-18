using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "ca-certificate", "list")]
public record AzSphereCaCertificateListOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;