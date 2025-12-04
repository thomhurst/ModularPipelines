using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "management-group", "subscription", "show")]
public record AzAccountManagementGroupSubscriptionShowOptions : AzOptions
{
    public AzAccountManagementGroupSubscriptionShowOptions(
        string name,
        string subscription
    )
    {
        Name = name;
        Subscription = subscription;
    }

    [CliOption("--name")]
    public string Name { get; set; }
}
