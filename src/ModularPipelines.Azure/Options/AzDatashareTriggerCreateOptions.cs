using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datashare", "trigger", "create")]
public record AzDatashareTriggerCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--share-subscription-name")] string ShareSubscriptionName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--scheduled-trigger")]
    public string? ScheduledTrigger { get; set; }
}