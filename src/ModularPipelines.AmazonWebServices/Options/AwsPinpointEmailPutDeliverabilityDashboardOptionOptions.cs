using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "put-deliverability-dashboard-option")]
public record AwsPinpointEmailPutDeliverabilityDashboardOptionOptions : AwsOptions
{
    [CliOption("--subscribed-domains")]
    public string[]? SubscribedDomains { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}