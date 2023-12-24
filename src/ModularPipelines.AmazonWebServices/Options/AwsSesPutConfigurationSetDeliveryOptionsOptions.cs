using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "put-configuration-set-delivery-options")]
public record AwsSesPutConfigurationSetDeliveryOptionsOptions(
[property: CommandSwitch("--configuration-set-name")] string ConfigurationSetName
) : AwsOptions
{
    [CommandSwitch("--delivery-options")]
    public string? DeliveryOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}