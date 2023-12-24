using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-feature-group")]
public record AwsSagemakerCreateFeatureGroupOptions(
[property: CommandSwitch("--feature-group-name")] string FeatureGroupName,
[property: CommandSwitch("--record-identifier-feature-name")] string RecordIdentifierFeatureName,
[property: CommandSwitch("--event-time-feature-name")] string EventTimeFeatureName,
[property: CommandSwitch("--feature-definitions")] string[] FeatureDefinitions
) : AwsOptions
{
    [CommandSwitch("--online-store-config")]
    public string? OnlineStoreConfig { get; set; }

    [CommandSwitch("--offline-store-config")]
    public string? OfflineStoreConfig { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}