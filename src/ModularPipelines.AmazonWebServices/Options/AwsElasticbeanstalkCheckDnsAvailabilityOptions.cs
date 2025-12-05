using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "check-dns-availability")]
public record AwsElasticbeanstalkCheckDnsAvailabilityOptions(
[property: CliOption("--cname-prefix")] string CnamePrefix
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}