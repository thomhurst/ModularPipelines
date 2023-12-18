using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managementpartner", "delete")]
public record AzManagementpartnerDeleteOptions(
[property: CommandSwitch("--partner-id")] string PartnerId
) : AzOptions;