using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-distribution")]
public record AwsCloudfrontCreateDistributionOptions : AwsOptions
{
    [CliOption("--distribution-config")]
    public string? DistributionConfig { get; set; }

    [CliOption("--origin-domain-name")]
    public string? OriginDomainName { get; set; }

    [CliOption("--default-root-object")]
    public string? DefaultRootObject { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}