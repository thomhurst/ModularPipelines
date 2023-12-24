using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "start-domain-maintenance")]
public record AwsOpensearchStartDomainMaintenanceOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--action")] string Action
) : AwsOptions
{
    [CommandSwitch("--node-id")]
    public string? NodeId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}