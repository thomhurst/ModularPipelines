using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "list-deleted")]
public record AzKeyvaultListDeletedOptions : AzOptions
{
    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}