using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "configuration", "show")]
public record AzEventgridPartnerConfigurationShowOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;