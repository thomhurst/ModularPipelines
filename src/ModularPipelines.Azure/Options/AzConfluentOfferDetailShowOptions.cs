using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("confluent", "offer-detail", "show")]
public record AzConfluentOfferDetailShowOptions : AzOptions
{
    [CommandSwitch("--offer-id")]
    public string? OfferId { get; set; }

    [CommandSwitch("--publisher-id")]
    public string? PublisherId { get; set; }
}