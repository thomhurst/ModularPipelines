using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("confluent", "organization", "list")]
public record AzConfluentOrganizationListOptions : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}