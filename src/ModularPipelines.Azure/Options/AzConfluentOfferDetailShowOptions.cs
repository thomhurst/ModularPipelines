using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("confluent", "offer-detail", "show")]
public record AzConfluentOfferDetailShowOptions : AzOptions
{
    [CliOption("--offer-id")]
    public string? OfferId { get; set; }

    [CliOption("--publisher-id")]
    public string? PublisherId { get; set; }
}