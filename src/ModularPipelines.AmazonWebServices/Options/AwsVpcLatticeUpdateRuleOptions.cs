using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "update-rule")]
public record AwsVpcLatticeUpdateRuleOptions(
[property: CommandSwitch("--listener-identifier")] string ListenerIdentifier,
[property: CommandSwitch("--rule-identifier")] string RuleIdentifier,
[property: CommandSwitch("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--match")]
    public string? Match { get; set; }

    [CommandSwitch("--priority")]
    public int? Priority { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}