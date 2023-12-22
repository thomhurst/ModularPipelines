using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "subscription", "add")]
public record AzAccountManagementGroupSubscriptionAddOptions : AzOptions
{
    public AzAccountManagementGroupSubscriptionAddOptions(
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
