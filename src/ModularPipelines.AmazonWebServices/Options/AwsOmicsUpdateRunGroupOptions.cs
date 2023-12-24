using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "update-run-group")]
public record AwsOmicsUpdateRunGroupOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--max-cpus")]
    public int? MaxCpus { get; set; }

    [CommandSwitch("--max-runs")]
    public int? MaxRuns { get; set; }

    [CommandSwitch("--max-duration")]
    public int? MaxDuration { get; set; }

    [CommandSwitch("--max-gpus")]
    public int? MaxGpus { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}