using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "product", "create")]
public record AzApimProductCreateOptions(
[property: CommandSwitch("--product-name")] string ProductName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [BooleanCommandSwitch("--approval-required")]
    public bool? ApprovalRequired { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--legal-terms")]
    public string? LegalTerms { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--product-id")]
    public string? ProductId { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [BooleanCommandSwitch("--subscription-required")]
    public bool? SubscriptionRequired { get; set; }

    [CommandSwitch("--subscriptions-limit")]
    public string? SubscriptionsLimit { get; set; }
}