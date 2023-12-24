using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "update-index")]
public record AwsKendraUpdateIndexOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--document-metadata-configuration-updates")]
    public string[]? DocumentMetadataConfigurationUpdates { get; set; }

    [CommandSwitch("--capacity-units")]
    public string? CapacityUnits { get; set; }

    [CommandSwitch("--user-token-configurations")]
    public string[]? UserTokenConfigurations { get; set; }

    [CommandSwitch("--user-context-policy")]
    public string? UserContextPolicy { get; set; }

    [CommandSwitch("--user-group-resolution-configuration")]
    public string? UserGroupResolutionConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}