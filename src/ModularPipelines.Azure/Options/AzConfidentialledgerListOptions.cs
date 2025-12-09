using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("confidentialledger", "list")]
public record AzConfidentialledgerListOptions : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}