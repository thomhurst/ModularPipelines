using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "namespace", "event-channel", "list", "(eventgrid", "extension)")]
public record AzEventgridPartnerNamespaceEventChannelListEventgridExtensionOptions(
[property: CliOption("--partner-namespace-name")] string PartnerNamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--odata-query")]
    public string? OdataQuery { get; set; }
}