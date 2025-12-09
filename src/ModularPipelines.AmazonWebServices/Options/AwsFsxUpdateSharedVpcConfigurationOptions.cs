using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "update-shared-vpc-configuration")]
public record AwsFsxUpdateSharedVpcConfigurationOptions : AwsOptions
{
    [CliOption("--enable-fsx-route-table-updates-from-participant-accounts")]
    public string? EnableFsxRouteTableUpdatesFromParticipantAccounts { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}