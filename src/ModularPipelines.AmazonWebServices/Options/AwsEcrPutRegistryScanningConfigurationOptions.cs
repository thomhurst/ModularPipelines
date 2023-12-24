using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "put-registry-scanning-configuration")]
public record AwsEcrPutRegistryScanningConfigurationOptions : AwsOptions
{
    [CommandSwitch("--scan-type")]
    public string? ScanType { get; set; }

    [CommandSwitch("--rules")]
    public string[]? Rules { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}