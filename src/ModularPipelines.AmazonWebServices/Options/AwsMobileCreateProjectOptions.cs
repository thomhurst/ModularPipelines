using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile", "create-project")]
public record AwsMobileCreateProjectOptions : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--contents")]
    public string? Contents { get; set; }

    [CommandSwitch("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CommandSwitch("--project-region")]
    public string? ProjectRegion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}