using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-email", "put-configuration-set-tracking-options")]
public record AwsPinpointEmailPutConfigurationSetTrackingOptionsOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName
) : AwsOptions
{
    [CommandSwitch("--custom-redirect-domain")]
    public string? CustomRedirectDomain { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}