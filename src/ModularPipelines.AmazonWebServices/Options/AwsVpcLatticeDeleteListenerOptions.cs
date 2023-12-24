using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "delete-listener")]
public record AwsVpcLatticeDeleteListenerOptions(
[property: CommandSwitch("--listener-identifier")] string ListenerIdentifier,
[property: CommandSwitch("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}