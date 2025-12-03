using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "upgrade-elasticsearch-domain")]
public record AwsEsUpgradeElasticsearchDomainOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--target-version")] string TargetVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}