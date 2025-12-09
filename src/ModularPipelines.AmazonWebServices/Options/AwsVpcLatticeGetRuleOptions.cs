using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "get-rule")]
public record AwsVpcLatticeGetRuleOptions(
[property: CliOption("--listener-identifier")] string ListenerIdentifier,
[property: CliOption("--rule-identifier")] string RuleIdentifier,
[property: CliOption("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}