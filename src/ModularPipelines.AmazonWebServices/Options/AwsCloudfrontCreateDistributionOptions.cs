using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-distribution")]
public record AwsCloudfrontCreateDistributionOptions : AwsOptions
{
    [CommandSwitch("--distribution-config")]
    public string? DistributionConfig { get; set; }

    [CommandSwitch("--origin-domain-name")]
    public string? OriginDomainName { get; set; }

    [CommandSwitch("--default-root-object")]
    public string? DefaultRootObject { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}