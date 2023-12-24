using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "update-app-version-app-component")]
public record AwsResiliencehubUpdateAppVersionAppComponentOptions(
[property: CommandSwitch("--app-arn")] string AppArn,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--additional-info")]
    public IEnumerable<KeyValue>? AdditionalInfo { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}