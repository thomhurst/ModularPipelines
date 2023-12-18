using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "data-connector", "update")]
public record AzSentinelDataConnectorUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [CommandSwitch("--data-connector-id")]
    public string? DataConnectorId { get; set; }

    [CommandSwitch("--defender-protection")]
    public string? DefenderProtection { get; set; }

    [CommandSwitch("--dynamics365")]
    public string? Dynamics365 { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--generic-ui")]
    public string? GenericUi { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

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

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--threat-intelligence")]
    public string? ThreatIntelligence { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}