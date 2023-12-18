using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managementpartner", "delete")]
public record AzManagementpartnerDeleteOptions(
[property: CommandSwitch("--partner-id")] string PartnerId
) : AzOptions
{
}