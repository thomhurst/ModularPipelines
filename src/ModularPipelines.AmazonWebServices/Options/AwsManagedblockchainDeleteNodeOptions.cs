using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedblockchain", "delete-node")]
public record AwsManagedblockchainDeleteNodeOptions(
[property: CommandSwitch("--network-id")] string NetworkId,
[property: CommandSwitch("--node-id")] string NodeId
) : AwsOptions
{
    [CommandSwitch("--member-id")]
    public string? MemberId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}