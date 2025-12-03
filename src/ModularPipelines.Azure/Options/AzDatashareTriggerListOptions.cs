using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datashare", "trigger", "list")]
public record AzDatashareTriggerListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--share-subscription-name")] string ShareSubscriptionName
) : AzOptions
{
    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }
}