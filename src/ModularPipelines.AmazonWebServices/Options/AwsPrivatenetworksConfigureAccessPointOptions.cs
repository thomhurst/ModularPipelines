using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privatenetworks", "configure-access-point")]
public record AwsPrivatenetworksConfigureAccessPointOptions(
[property: CommandSwitch("--access-point-arn")] string AccessPointArn
) : AwsOptions
{
    [CommandSwitch("--cpi-secret-key")]
    public string? CpiSecretKey { get; set; }

    [CommandSwitch("--cpi-user-id")]
    public string? CpiUserId { get; set; }

    [CommandSwitch("--cpi-user-password")]
    public string? CpiUserPassword { get; set; }

    [CommandSwitch("--cpi-username")]
    public string? CpiUsername { get; set; }

    [CommandSwitch("--position")]
    public string? Position { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}