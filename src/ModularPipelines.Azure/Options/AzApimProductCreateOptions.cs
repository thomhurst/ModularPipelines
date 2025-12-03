using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "product", "create")]
public record AzApimProductCreateOptions(
[property: CliOption("--product-name")] string ProductName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliFlag("--approval-required")]
    public bool? ApprovalRequired { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--legal-terms")]
    public string? LegalTerms { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--product-id")]
    public string? ProductId { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliFlag("--subscription-required")]
    public bool? SubscriptionRequired { get; set; }

    [CliOption("--subscriptions-limit")]
    public string? SubscriptionsLimit { get; set; }
}