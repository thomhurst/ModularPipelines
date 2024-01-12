using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "consent-stores", "query-accessible-data")]
public record GcloudHealthcareConsentStoresQueryAccessibleDataOptions(
[property: PositionalArgument] string ConsentStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--gcs-uri")] string GcsUri
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--request-attributes")]
    public IEnumerable<KeyValue>? RequestAttributes { get; set; }

    [CommandSwitch("--resource-attributes")]
    public IEnumerable<KeyValue>? ResourceAttributes { get; set; }
}