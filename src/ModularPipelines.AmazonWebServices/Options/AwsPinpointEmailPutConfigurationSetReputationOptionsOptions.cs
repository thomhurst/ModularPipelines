using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-email", "put-configuration-set-reputation-options")]
public record AwsPinpointEmailPutConfigurationSetReputationOptionsOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}