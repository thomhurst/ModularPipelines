using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "put-environment-blueprint-configuration")]
public record AwsDatazonePutEnvironmentBlueprintConfigurationOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--enabled-regions")] string[] EnabledRegions,
[property: CliOption("--environment-blueprint-identifier")] string EnvironmentBlueprintIdentifier
) : AwsOptions
{
    [CliOption("--manage-access-role-arn")]
    public string? ManageAccessRoleArn { get; set; }

    [CliOption("--provisioning-role-arn")]
    public string? ProvisioningRoleArn { get; set; }

    [CliOption("--regional-parameters")]
    public IEnumerable<KeyValue>? RegionalParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}