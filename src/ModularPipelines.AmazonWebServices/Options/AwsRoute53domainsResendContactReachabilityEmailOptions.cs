using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53domains", "resend-contact-reachability-email")]
public record AwsRoute53domainsResendContactReachabilityEmailOptions : AwsOptions
{
    [CommandSwitch("--domain-name")]
    public string? DomainName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}