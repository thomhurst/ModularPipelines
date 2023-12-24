using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-ip-restriction")]
public record AwsQuicksightUpdateIpRestrictionOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId
) : AwsOptions
{
    [CommandSwitch("--ip-restriction-rule-map")]
    public IEnumerable<KeyValue>? IpRestrictionRuleMap { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}