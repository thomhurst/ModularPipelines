using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "create-auto-scaling-configuration")]
public record AwsApprunnerCreateAutoScalingConfigurationOptions(
[property: CliOption("--auto-scaling-configuration-name")] string AutoScalingConfigurationName
) : AwsOptions
{
    [CliOption("--max-concurrency")]
    public int? MaxConcurrency { get; set; }

    [CliOption("--min-size")]
    public int? MinSize { get; set; }

    [CliOption("--max-size")]
    public int? MaxSize { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}