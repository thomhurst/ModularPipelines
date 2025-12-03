using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "product", "delete")]
public record AzApimProductDeleteOptions(
[property: CliOption("--product-id")] string ProductId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--delete-subscriptions")]
    public string? DeleteSubscriptions { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}