using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "update-compute-environment")]
public record AwsBatchUpdateComputeEnvironmentOptions(
[property: CommandSwitch("--compute-environment")] string ComputeEnvironment
) : AwsOptions
{
    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--unmanagedv-cpus")]
    public int? UnmanagedvCpus { get; set; }

    [CommandSwitch("--compute-resources")]
    public string? ComputeResources { get; set; }

    [CommandSwitch("--service-role")]
    public string? ServiceRole { get; set; }

    [CommandSwitch("--update-policy")]
    public string? UpdatePolicy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}