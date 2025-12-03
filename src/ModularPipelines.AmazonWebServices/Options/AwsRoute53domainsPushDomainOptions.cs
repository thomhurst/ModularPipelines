using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "push-domain")]
public record AwsRoute53domainsPushDomainOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--target")] string Target
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}