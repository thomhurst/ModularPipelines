using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managementpartner", "create")]
public record AzManagementpartnerCreateOptions(
[property: CommandSwitch("--partner-id")] string PartnerId
) : AzOptions;