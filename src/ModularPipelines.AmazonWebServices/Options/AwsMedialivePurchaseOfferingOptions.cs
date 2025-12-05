using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "purchase-offering")]
public record AwsMedialivePurchaseOfferingOptions(
[property: CliOption("--count")] int Count,
[property: CliOption("--offering-id")] string OfferingId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--renewal-settings")]
    public string? RenewalSettings { get; set; }

    [CliOption("--request-id")]
    public string? RequestId { get; set; }

    [CliOption("--start")]
    public string? Start { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}