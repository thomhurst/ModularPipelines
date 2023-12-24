using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "provision-byoip-cidr")]
public record AwsGlobalacceleratorProvisionByoipCidrOptions(
[property: CommandSwitch("--cidr")] string Cidr,
[property: CommandSwitch("--cidr-authorization-context")] string CidrAuthorizationContext
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}