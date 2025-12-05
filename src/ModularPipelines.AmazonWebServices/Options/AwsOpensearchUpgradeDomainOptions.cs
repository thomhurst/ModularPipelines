using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "upgrade-domain")]
public record AwsOpensearchUpgradeDomainOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--target-version")] string TargetVersion
) : AwsOptions
{
    [CliOption("--advanced-options")]
    public IEnumerable<KeyValue>? AdvancedOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}