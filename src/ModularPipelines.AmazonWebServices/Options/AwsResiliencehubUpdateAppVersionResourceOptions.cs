using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "update-app-version-resource")]
public record AwsResiliencehubUpdateAppVersionResourceOptions(
[property: CommandSwitch("--app-arn")] string AppArn
) : AwsOptions
{
    [CommandSwitch("--additional-info")]
    public IEnumerable<KeyValue>? AdditionalInfo { get; set; }

    [CommandSwitch("--app-components")]
    public string[]? AppComponents { get; set; }

    [CommandSwitch("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CommandSwitch("--aws-region")]
    public string? AwsRegion { get; set; }

    [CommandSwitch("--logical-resource-id")]
    public string? LogicalResourceId { get; set; }

    [CommandSwitch("--physical-resource-id")]
    public string? PhysicalResourceId { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}