using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "update-shared-vpc-configuration")]
public record AwsFsxUpdateSharedVpcConfigurationOptions : AwsOptions
{
    [CommandSwitch("--enable-fsx-route-table-updates-from-participant-accounts")]
    public string? EnableFsxRouteTableUpdatesFromParticipantAccounts { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}