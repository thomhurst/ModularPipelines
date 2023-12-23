using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "create-auto-scaling-configuration")]
public record AwsApprunnerCreateAutoScalingConfigurationOptions(
[property: CommandSwitch("--auto-scaling-configuration-name")] string AutoScalingConfigurationName
) : AwsOptions
{
    [CommandSwitch("--max-concurrency")]
    public int? MaxConcurrency { get; set; }

    [CommandSwitch("--min-size")]
    public int? MinSize { get; set; }

    [CommandSwitch("--max-size")]
    public int? MaxSize { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}