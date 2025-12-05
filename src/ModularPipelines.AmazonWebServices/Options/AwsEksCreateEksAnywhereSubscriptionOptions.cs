using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "create-eks-anywhere-subscription")]
public record AwsEksCreateEksAnywhereSubscriptionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--term")] string Term
) : AwsOptions
{
    [CliOption("--license-quantity")]
    public int? LicenseQuantity { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}