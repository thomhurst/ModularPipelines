using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managementpartner", "update")]
public record AzManagementpartnerUpdateOptions(
[property: CommandSwitch("--partner-id")] string PartnerId
) : AzOptions;