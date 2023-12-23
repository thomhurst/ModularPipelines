using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "get-domain-maintenance-status")]
public record AwsOpensearchGetDomainMaintenanceStatusOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--maintenance-id")] string MaintenanceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}