using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "data-connector", "update")]
public record AzSentinelDataConnectorUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--api-polling")]
    public string? ApiPolling { get; set; }

    [CliOption("--aws-cloud-trail")]
    public string? AwsCloudTrail { get; set; }

    [CliOption("--aws-s3")]
    public string? AwsS3 { get; set; }

    [CliOption("--azure-active-directory")]
    public string? AzureActiveDirectory { get; set; }

    [CliOption("--azure-protection")]
    public string? AzureProtection { get; set; }

    [CliOption("--azure-security-center")]
    public string? AzureSecurityCenter { get; set; }

    [CliOption("--cloud-app-security")]
    public string? CloudAppSecurity { get; set; }

    [CliOption("--data-connector-id")]
    public string? DataConnectorId { get; set; }

    [CliOption("--defender-protection")]
    public string? DefenderProtection { get; set; }

    [CliOption("--dynamics365")]
    public string? Dynamics365 { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--generic-ui")]
    public string? GenericUi { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--intelligence-taxii")]
    public string? IntelligenceTaxii { get; set; }

    [CliOption("--iot")]
    public string? Iot { get; set; }

    [CliOption("--microsoft-intelligence")]
    public string? MicrosoftIntelligence { get; set; }

    [CliOption("--microsoft-protection")]
    public string? MicrosoftProtection { get; set; }

    [CliOption("--office-atp")]
    public string? OfficeAtp { get; set; }

    [CliOption("--office-irm")]
    public string? OfficeIrm { get; set; }

    [CliOption("--office-power-bi")]
    public string? OfficePowerBi { get; set; }

    [CliOption("--office365")]
    public string? Office365 { get; set; }

    [CliOption("--office365-project")]
    public string? Office365Project { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--threat-intelligence")]
    public string? ThreatIntelligence { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}