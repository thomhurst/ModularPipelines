using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "data-connector", "create")]
public record AzSentinelDataConnectorCreateOptions(
[property: CommandSwitch("--data-connector-id")] string DataConnectorId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--api-polling")]
    public string? ApiPolling { get; set; }

    [CommandSwitch("--aws-cloud-trail")]
    public string? AwsCloudTrail { get; set; }

    [CommandSwitch("--aws-s3")]
    public string? AwsS3 { get; set; }

    [CommandSwitch("--azure-active-directory")]
    public string? AzureActiveDirectory { get; set; }

    [CommandSwitch("--azure-protection")]
    public string? AzureProtection { get; set; }

    [CommandSwitch("--azure-security-center")]
    public string? AzureSecurityCenter { get; set; }

    [CommandSwitch("--cloud-app-security")]
    public string? CloudAppSecurity { get; set; }

    [CommandSwitch("--defender-protection")]
    public string? DefenderProtection { get; set; }

    [CommandSwitch("--dynamics365")]
    public string? Dynamics365 { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--generic-ui")]
    public string? GenericUi { get; set; }

    [CommandSwitch("--intelligence-taxii")]
    public string? IntelligenceTaxii { get; set; }

    [CommandSwitch("--iot")]
    public string? Iot { get; set; }

    [CommandSwitch("--microsoft-intelligence")]
    public string? MicrosoftIntelligence { get; set; }

    [CommandSwitch("--microsoft-protection")]
    public string? MicrosoftProtection { get; set; }

    [CommandSwitch("--office-atp")]
    public string? OfficeAtp { get; set; }

    [CommandSwitch("--office-irm")]
    public string? OfficeIrm { get; set; }

    [CommandSwitch("--office-power-bi")]
    public string? OfficePowerBi { get; set; }

    [CommandSwitch("--office365")]
    public string? Office365 { get; set; }

    [CommandSwitch("--office365-project")]
    public string? Office365Project { get; set; }

    [CommandSwitch("--threat-intelligence")]
    public string? ThreatIntelligence { get; set; }
}