using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "data-connector", "create")]
public record AzSentinelDataConnectorCreateOptions(
[property: CliOption("--data-connector-id")] string DataConnectorId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
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

    [CliOption("--defender-protection")]
    public string? DefenderProtection { get; set; }

    [CliOption("--dynamics365")]
    public string? Dynamics365 { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--generic-ui")]
    public string? GenericUi { get; set; }

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

    [CliOption("--threat-intelligence")]
    public string? ThreatIntelligence { get; set; }
}