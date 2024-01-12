using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "consent-stores", "evaluate-user-consents")]
public record GcloudHealthcareConsentStoresEvaluateUserConsentsOptions(
[property: PositionalArgument] string ConsentStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--user-id")] string UserId
) : GcloudOptions
{
    [CommandSwitch("--consent-list")]
    public string[]? ConsentList { get; set; }

    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }

    [CommandSwitch("--request-attributes")]
    public IEnumerable<KeyValue>? RequestAttributes { get; set; }

    [CommandSwitch("--resource-attributes")]
    public IEnumerable<KeyValue>? ResourceAttributes { get; set; }

    [CommandSwitch("--response-view")]
    public string? ResponseView { get; set; }
}