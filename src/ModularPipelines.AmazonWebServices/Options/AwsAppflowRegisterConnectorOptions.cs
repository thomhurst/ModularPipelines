using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appflow", "register-connector")]
public record AwsAppflowRegisterConnectorOptions : AwsOptions
{
    [CliOption("--connector-label")]
    public string? ConnectorLabel { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--connector-provisioning-type")]
    public string? ConnectorProvisioningType { get; set; }

    [CliOption("--connector-provisioning-config")]
    public string? ConnectorProvisioningConfig { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}