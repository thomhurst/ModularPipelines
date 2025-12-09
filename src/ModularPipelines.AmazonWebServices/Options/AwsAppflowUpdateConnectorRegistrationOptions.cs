using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appflow", "update-connector-registration")]
public record AwsAppflowUpdateConnectorRegistrationOptions(
[property: CliOption("--connector-label")] string ConnectorLabel
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--connector-provisioning-config")]
    public string? ConnectorProvisioningConfig { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}