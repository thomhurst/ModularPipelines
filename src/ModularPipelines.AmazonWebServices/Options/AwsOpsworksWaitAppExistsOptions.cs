using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "wait", "app-exists")]
public record AwsOpsworksWaitAppExistsOptions : AwsOptions
{
    [CommandSwitch("--stack-id")]
    public string? StackId { get; set; }

    [CommandSwitch("--app-ids")]
    public string[]? AppIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}