using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedblockchain", "create-member")]
public record AwsManagedblockchainCreateMemberOptions(
[property: CommandSwitch("--invitation-id")] string InvitationId,
[property: CommandSwitch("--network-id")] string NetworkId,
[property: CommandSwitch("--member-configuration")] string MemberConfiguration
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}