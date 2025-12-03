using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "describe-instance-type-limits")]
public record AwsOpensearchDescribeInstanceTypeLimitsOptions(
[property: CliOption("--instance-type")] string InstanceType,
[property: CliOption("--engine-version")] string EngineVersion
) : AwsOptions
{
    [CliOption("--domain-name")]
    public string? DomainName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}