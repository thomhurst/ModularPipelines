using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rbin", "lock-rule")]
public record AwsRbinLockRuleOptions(
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--lock-configuration")] string LockConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}