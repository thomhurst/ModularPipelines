using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "update-rule")]
public record AwsVpcLatticeUpdateRuleOptions(
[property: CliOption("--listener-identifier")] string ListenerIdentifier,
[property: CliOption("--rule-identifier")] string RuleIdentifier,
[property: CliOption("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--match")]
    public string? Match { get; set; }

    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}