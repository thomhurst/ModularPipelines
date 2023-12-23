using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "describe-instance-type-limits")]
public record AwsOpensearchDescribeInstanceTypeLimitsOptions(
[property: CommandSwitch("--instance-type")] string InstanceType,
[property: CommandSwitch("--engine-version")] string EngineVersion
) : AwsOptions
{
    [CommandSwitch("--domain-name")]
    public string? DomainName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}