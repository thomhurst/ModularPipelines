using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "put-deliverability-dashboard-option")]
public record AwsSesv2PutDeliverabilityDashboardOptionOptions : AwsOptions
{
    [CommandSwitch("--subscribed-domains")]
    public string[]? SubscribedDomains { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}