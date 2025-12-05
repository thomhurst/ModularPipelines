using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicediscovery", "update-private-dns-namespace")]
public record AwsServicediscoveryUpdatePrivateDnsNamespaceOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--namespace")] string Namespace
) : AwsOptions
{
    [CliOption("--updater-request-id")]
    public string? UpdaterRequestId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}