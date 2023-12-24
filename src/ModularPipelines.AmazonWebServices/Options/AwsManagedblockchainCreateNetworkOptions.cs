using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedblockchain", "create-network")]
public record AwsManagedblockchainCreateNetworkOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--framework")] string Framework,
[property: CommandSwitch("--framework-version")] string FrameworkVersion,
[property: CommandSwitch("--voting-policy")] string VotingPolicy,
[property: CommandSwitch("--member-configuration")] string MemberConfiguration
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--framework-configuration")]
    public string? FrameworkConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}