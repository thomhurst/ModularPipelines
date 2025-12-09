using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "update-listener")]
public record AwsVpcLatticeUpdateListenerOptions(
[property: CliOption("--default-action")] string DefaultAction,
[property: CliOption("--listener-identifier")] string ListenerIdentifier,
[property: CliOption("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}