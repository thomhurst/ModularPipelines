using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "resend-contact-reachability-email")]
public record AwsRoute53domainsResendContactReachabilityEmailOptions : AwsOptions
{
    [CliOption("--domain-name")]
    public string? DomainName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}