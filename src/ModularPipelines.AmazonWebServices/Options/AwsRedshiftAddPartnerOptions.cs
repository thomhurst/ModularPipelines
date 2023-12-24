using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "add-partner")]
public record AwsRedshiftAddPartnerOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--partner-name")] string PartnerName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}