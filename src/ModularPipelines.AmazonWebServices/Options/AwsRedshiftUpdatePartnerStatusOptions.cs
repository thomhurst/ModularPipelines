using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "update-partner-status")]
public record AwsRedshiftUpdatePartnerStatusOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--partner-name")] string PartnerName,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--status-message")]
    public string? StatusMessage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}