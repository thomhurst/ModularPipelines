using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "create-app-version-resource")]
public record AwsResiliencehubCreateAppVersionResourceOptions(
[property: CommandSwitch("--app-arn")] string AppArn,
[property: CommandSwitch("--app-components")] string[] AppComponents,
[property: CommandSwitch("--logical-resource-id")] string LogicalResourceId,
[property: CommandSwitch("--physical-resource-id")] string PhysicalResourceId,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AwsOptions
{
    [CommandSwitch("--additional-info")]
    public IEnumerable<KeyValue>? AdditionalInfo { get; set; }

    [CommandSwitch("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CommandSwitch("--aws-region")]
    public string? AwsRegion { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}