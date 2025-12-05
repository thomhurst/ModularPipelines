using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "topic", "list", "(eventgrid", "extension)")]
public record AzEventgridPartnerTopicListEventgridExtensionOptions : AzOptions
{
    [CliOption("--odata-query")]
    public string? OdataQuery { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}