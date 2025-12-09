using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privatenetworks", "configure-access-point")]
public record AwsPrivatenetworksConfigureAccessPointOptions(
[property: CliOption("--access-point-arn")] string AccessPointArn
) : AwsOptions
{
    [CliOption("--cpi-secret-key")]
    public string? CpiSecretKey { get; set; }

    [CliOption("--cpi-user-id")]
    public string? CpiUserId { get; set; }

    [CliOption("--cpi-user-password")]
    public string? CpiUserPassword { get; set; }

    [CliOption("--cpi-username")]
    public string? CpiUsername { get; set; }

    [CliOption("--position")]
    public string? Position { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}