using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain", "create-proposal")]
public record AwsManagedblockchainCreateProposalOptions(
[property: CliOption("--network-id")] string NetworkId,
[property: CliOption("--member-id")] string MemberId,
[property: CliOption("--actions")] string Actions
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}