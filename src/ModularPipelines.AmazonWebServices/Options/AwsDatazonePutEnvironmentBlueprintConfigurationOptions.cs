using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "put-environment-blueprint-configuration")]
public record AwsDatazonePutEnvironmentBlueprintConfigurationOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--enabled-regions")] string[] EnabledRegions,
[property: CommandSwitch("--environment-blueprint-identifier")] string EnvironmentBlueprintIdentifier
) : AwsOptions
{
    [CommandSwitch("--manage-access-role-arn")]
    public string? ManageAccessRoleArn { get; set; }

    [CommandSwitch("--provisioning-role-arn")]
    public string? ProvisioningRoleArn { get; set; }

    [CommandSwitch("--regional-parameters")]
    public IEnumerable<KeyValue>? RegionalParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}