using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-email", "put-deliverability-dashboard-option")]
public record AwsPinpointEmailPutDeliverabilityDashboardOptionOptions : AwsOptions
{
    [CommandSwitch("--subscribed-domains")]
    public string[]? SubscribedDomains { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}