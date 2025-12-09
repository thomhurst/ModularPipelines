using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "alias", "create")]
public record AzAccountAliasCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--billing-scope")]
    public string? BillingScope { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--reseller-id")]
    public string? ResellerId { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--workload")]
    public string? Workload { get; set; }
}