using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "create-compute-environment")]
public record AwsBatchCreateComputeEnvironmentOptions(
[property: CommandSwitch("--compute-environment-name")] string ComputeEnvironmentName,
[property: CommandSwitch("--type")] string Type
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

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--eks-configuration")]
    public string? EksConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}