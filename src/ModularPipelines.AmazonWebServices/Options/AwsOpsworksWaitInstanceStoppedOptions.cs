using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "wait", "instance-stopped")]
public record AwsOpsworksWaitInstanceStoppedOptions : AwsOptions
{
    [CliOption("--stack-id")]
    public string? StackId { get; set; }

    [CliOption("--layer-id")]
    public string? LayerId { get; set; }

    [CliOption("--instance-ids")]
    public string[]? InstanceIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}