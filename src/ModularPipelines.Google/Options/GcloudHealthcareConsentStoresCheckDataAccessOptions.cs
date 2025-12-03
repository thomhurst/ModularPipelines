using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "consent-stores", "check-data-access")]
public record GcloudHealthcareConsentStoresCheckDataAccessOptions(
[property: CliArgument] string ConsentStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliOption("--data-id")] string DataId
) : GcloudOptions
{
    [CliOption("--request-attributes")]
    public IEnumerable<KeyValue>? RequestAttributes { get; set; }
}