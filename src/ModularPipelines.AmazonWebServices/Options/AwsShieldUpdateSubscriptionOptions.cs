using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("shield", "update-subscription")]
public record AwsShieldUpdateSubscriptionOptions : AwsOptions
{
    [CommandSwitch("--auto-renew")]
    public string? AutoRenew { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}