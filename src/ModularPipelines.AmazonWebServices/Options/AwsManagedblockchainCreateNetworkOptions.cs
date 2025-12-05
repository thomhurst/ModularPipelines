using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain", "create-network")]
public record AwsManagedblockchainCreateNetworkOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--framework")] string Framework,
[property: CliOption("--framework-version")] string FrameworkVersion,
[property: CliOption("--voting-policy")] string VotingPolicy,
[property: CliOption("--member-configuration")] string MemberConfiguration
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--framework-configuration")]
    public string? FrameworkConfiguration { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}