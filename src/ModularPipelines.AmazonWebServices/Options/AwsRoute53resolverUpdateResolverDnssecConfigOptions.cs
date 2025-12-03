using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "update-resolver-dnssec-config")]
public record AwsRoute53resolverUpdateResolverDnssecConfigOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--validation")] string Validation
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}