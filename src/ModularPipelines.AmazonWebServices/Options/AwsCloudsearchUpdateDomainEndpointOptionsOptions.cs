using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearch", "update-domain-endpoint-options")]
public record AwsCloudsearchUpdateDomainEndpointOptionsOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--domain-endpoint-options")] string DomainEndpointOptions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}