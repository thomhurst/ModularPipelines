using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "get-core-network-policy")]
public record AwsNetworkmanagerGetCoreNetworkPolicyOptions(
[property: CommandSwitch("--core-network-id")] string CoreNetworkId
) : AwsOptions
{
    [CommandSwitch("--policy-version-id")]
    public int? PolicyVersionId { get; set; }

    [CommandSwitch("--alias")]
    public string? Alias { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}