using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "create-deployment")]
public record AwsOpsworksCreateDeploymentOptions(
[property: CliOption("--stack-id")] string StackId,
[property: CliOption("--command")] string Command
) : AwsOptions
{
    [CliOption("--app-id")]
    public string? AppId { get; set; }

    [CliOption("--instance-ids")]
    public string[]? InstanceIds { get; set; }

    [CliOption("--layer-ids")]
    public string[]? LayerIds { get; set; }

    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--custom-json")]
    public string? CustomJson { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}