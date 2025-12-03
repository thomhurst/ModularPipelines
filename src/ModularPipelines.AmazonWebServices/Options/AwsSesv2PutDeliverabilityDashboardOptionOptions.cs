using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "put-deliverability-dashboard-option")]
public record AwsSesv2PutDeliverabilityDashboardOptionOptions : AwsOptions
{
    [CliOption("--subscribed-domains")]
    public string[]? SubscribedDomains { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}