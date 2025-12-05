using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-gateway", "create-gateway")]
public record AwsBackupGatewayCreateGatewayOptions(
[property: CliOption("--activation-key")] string ActivationKey,
[property: CliOption("--gateway-display-name")] string GatewayDisplayName,
[property: CliOption("--gateway-type")] string GatewayType
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}