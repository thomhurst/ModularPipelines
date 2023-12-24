using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "execute-core-network-change-set")]
public record AwsNetworkmanagerExecuteCoreNetworkChangeSetOptions(
[property: CommandSwitch("--core-network-id")] string CoreNetworkId,
[property: CommandSwitch("--policy-version-id")] int PolicyVersionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}