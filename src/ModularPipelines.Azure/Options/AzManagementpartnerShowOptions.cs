using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managementpartner", "show")]
public record AzManagementpartnerShowOptions : AzOptions
{
    [CliOption("--partner-id")]
    public string? PartnerId { get; set; }
}