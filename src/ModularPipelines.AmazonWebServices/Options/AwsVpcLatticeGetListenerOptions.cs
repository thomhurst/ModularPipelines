using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "get-listener")]
public record AwsVpcLatticeGetListenerOptions(
[property: CliOption("--listener-identifier")] string ListenerIdentifier,
[property: CliOption("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}