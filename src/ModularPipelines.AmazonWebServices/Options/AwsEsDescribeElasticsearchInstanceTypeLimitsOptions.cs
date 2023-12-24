using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "describe-elasticsearch-instance-type-limits")]
public record AwsEsDescribeElasticsearchInstanceTypeLimitsOptions(
[property: CommandSwitch("--instance-type")] string InstanceType,
[property: CommandSwitch("--elasticsearch-version")] string ElasticsearchVersion
) : AwsOptions
{
    [CommandSwitch("--domain-name")]
    public string? DomainName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}