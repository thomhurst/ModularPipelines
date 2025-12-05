using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appflow", "create-connector-profile")]
public record AwsAppflowCreateConnectorProfileOptions(
[property: CliOption("--connector-profile-name")] string ConnectorProfileName,
[property: CliOption("--connector-type")] string ConnectorType,
[property: CliOption("--connection-mode")] string ConnectionMode,
[property: CliOption("--connector-profile-config")] string ConnectorProfileConfig
) : AwsOptions
{
    [CliOption("--kms-arn")]
    public string? KmsArn { get; set; }

    [CliOption("--connector-label")]
    public string? ConnectorLabel { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}