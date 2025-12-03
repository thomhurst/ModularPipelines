using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "consent-stores", "query-accessible-data")]
public record GcloudHealthcareConsentStoresQueryAccessibleDataOptions(
[property: CliArgument] string ConsentStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliOption("--gcs-uri")] string GcsUri
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--request-attributes")]
    public IEnumerable<KeyValue>? RequestAttributes { get; set; }

    [CliOption("--resource-attributes")]
    public IEnumerable<KeyValue>? ResourceAttributes { get; set; }
}