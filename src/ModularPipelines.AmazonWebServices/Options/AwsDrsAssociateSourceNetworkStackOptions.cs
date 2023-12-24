using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("drs", "associate-source-network-stack")]
public record AwsDrsAssociateSourceNetworkStackOptions(
[property: CommandSwitch("--cfn-stack-name")] string CfnStackName,
[property: CommandSwitch("--source-network-id")] string SourceNetworkId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}