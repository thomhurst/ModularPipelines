using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "consent-stores", "evaluate-user-consents")]
public record GcloudHealthcareConsentStoresEvaluateUserConsentsOptions(
[property: CliArgument] string ConsentStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliOption("--user-id")] string UserId
) : GcloudOptions
{
    [CliOption("--consent-list")]
    public string[]? ConsentList { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--request-attributes")]
    public IEnumerable<KeyValue>? RequestAttributes { get; set; }

    [CliOption("--resource-attributes")]
    public IEnumerable<KeyValue>? ResourceAttributes { get; set; }

    [CliOption("--response-view")]
    public string? ResponseView { get; set; }
}