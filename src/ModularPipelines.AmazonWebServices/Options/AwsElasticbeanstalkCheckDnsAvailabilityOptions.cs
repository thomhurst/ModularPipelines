using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "check-dns-availability")]
public record AwsElasticbeanstalkCheckDnsAvailabilityOptions(
[property: CommandSwitch("--cname-prefix")] string CnamePrefix
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}