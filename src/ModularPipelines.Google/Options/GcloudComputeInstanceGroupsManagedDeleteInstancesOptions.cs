using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "delete-instances")]
public record GcloudComputeInstanceGroupsManagedDeleteInstancesOptions(
[property: CliArgument] string Name,
[property: CliOption("--instances")] string[] Instances
) : GcloudOptions
{
    [CliFlag("--skip-instances-on-validation-error")]
    public bool? SkipInstancesOnValidationError { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}