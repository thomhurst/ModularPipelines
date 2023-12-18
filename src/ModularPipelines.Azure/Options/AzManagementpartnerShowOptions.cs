using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managementpartner", "show")]
public record AzManagementpartnerShowOptions(
[property: CommandSwitch("--partner-id")] string PartnerId
) : AzOptions
{
}

