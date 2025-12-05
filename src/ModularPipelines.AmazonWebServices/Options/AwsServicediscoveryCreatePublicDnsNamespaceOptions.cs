using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicediscovery", "create-public-dns-namespace")]
public record AwsServicediscoveryCreatePublicDnsNamespaceOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}