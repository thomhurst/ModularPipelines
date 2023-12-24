using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "describe-router-configuration")]
public record AwsDirectconnectDescribeRouterConfigurationOptions(
[property: CommandSwitch("--virtual-interface-id")] string VirtualInterfaceId
) : AwsOptions
{
    [CommandSwitch("--router-type-identifier")]
    public string? RouterTypeIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}