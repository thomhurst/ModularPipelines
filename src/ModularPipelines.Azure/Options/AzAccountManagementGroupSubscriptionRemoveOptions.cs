using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "subscription", "remove")]
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

    [CommandSwitch("--name")]
    public string Name { get; set; }
}
