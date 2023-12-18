using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managementpartner", "show")]
public record AzManagementpartnerShowOptions : AzOptions
{
    [CommandSwitch("--partner-id")]
    public string? PartnerId { get; set; }
}