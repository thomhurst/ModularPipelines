using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appflow", "update-connector-profile")]
public record AwsAppflowUpdateConnectorProfileOptions(
[property: CliOption("--connector-profile-name")] string ConnectorProfileName,
[property: CliOption("--connection-mode")] string ConnectionMode,
[property: CliOption("--connector-profile-config")] string ConnectorProfileConfig
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}