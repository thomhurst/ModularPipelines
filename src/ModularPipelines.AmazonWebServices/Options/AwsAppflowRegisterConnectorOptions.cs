using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appflow", "register-connector")]
public record AwsAppflowRegisterConnectorOptions : AwsOptions
{
    [CommandSwitch("--connector-label")]
    public string? ConnectorLabel { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--connector-provisioning-type")]
    public string? ConnectorProvisioningType { get; set; }

    [CommandSwitch("--connector-provisioning-config")]
    public string? ConnectorProvisioningConfig { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}