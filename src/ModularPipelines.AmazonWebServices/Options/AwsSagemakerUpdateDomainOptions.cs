using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-domain")]
public record AwsSagemakerUpdateDomainOptions(
[property: CommandSwitch("--domain-id")] string DomainId
) : AwsOptions
{
    [CommandSwitch("--default-user-settings")]
    public string? DefaultUserSettings { get; set; }

    [CommandSwitch("--domain-settings-for-update")]
    public string? DomainSettingsForUpdate { get; set; }

    [CommandSwitch("--app-security-group-management")]
    public string? AppSecurityGroupManagement { get; set; }

    [CommandSwitch("--default-space-settings")]
    public string? DefaultSpaceSettings { get; set; }

    [CommandSwitch("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CommandSwitch("--app-network-access-type")]
    public string? AppNetworkAccessType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}