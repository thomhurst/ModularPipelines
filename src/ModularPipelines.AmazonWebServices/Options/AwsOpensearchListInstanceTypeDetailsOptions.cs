using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "list-instance-type-details")]
public record AwsOpensearchListInstanceTypeDetailsOptions(
[property: CommandSwitch("--engine-version")] string EngineVersion
) : AwsOptions
{
    [CommandSwitch("--domain-name")]
    public string? DomainName { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}