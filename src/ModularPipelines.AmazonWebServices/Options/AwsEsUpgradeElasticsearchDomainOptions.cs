using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "upgrade-elasticsearch-domain")]
public record AwsEsUpgradeElasticsearchDomainOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--target-version")] string TargetVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}