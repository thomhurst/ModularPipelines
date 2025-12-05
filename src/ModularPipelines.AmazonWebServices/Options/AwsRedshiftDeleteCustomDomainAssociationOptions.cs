using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "delete-custom-domain-association")]
public record AwsRedshiftDeleteCustomDomainAssociationOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier,
[property: CliOption("--custom-domain-name")] string CustomDomainName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}