using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "batch-update-rule")]
public record AwsVpcLatticeBatchUpdateRuleOptions(
[property: CommandSwitch("--listener-identifier")] string ListenerIdentifier,
[property: CommandSwitch("--rules")] string[] Rules,
[property: CommandSwitch("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}