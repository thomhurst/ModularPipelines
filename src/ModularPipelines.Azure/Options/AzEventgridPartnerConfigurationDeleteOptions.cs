using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "configuration", "delete")]
public record AzEventgridPartnerConfigurationDeleteOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}