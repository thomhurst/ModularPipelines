using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "management-group", "subscription", "remove")]
public record AzAccountManagementGroupSubscriptionRemoveOptions : AzOptions
{
    public AzAccountManagementGroupSubscriptionRemoveOptions(
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
