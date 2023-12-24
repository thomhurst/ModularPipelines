using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "create-rule")]
public record AwsVpcLatticeCreateRuleOptions(
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--listener-identifier")] string ListenerIdentifier,
[property: CommandSwitch("--match")] string Match,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--priority")] int Priority,
[property: CommandSwitch("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}