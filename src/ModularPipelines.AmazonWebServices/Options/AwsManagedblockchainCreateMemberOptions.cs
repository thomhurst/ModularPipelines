using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain", "create-member")]
public record AwsManagedblockchainCreateMemberOptions(
[property: CliOption("--invitation-id")] string InvitationId,
[property: CliOption("--network-id")] string NetworkId,
[property: CliOption("--member-configuration")] string MemberConfiguration
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}