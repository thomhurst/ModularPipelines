using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "get-reusable-delegation-set-limit")]
public record AwsRoute53GetReusableDelegationSetLimitOptions(
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--delegation-set-id")] string DelegationSetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}