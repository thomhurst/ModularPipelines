using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "create-observability-configuration")]
public record AwsApprunnerCreateObservabilityConfigurationOptions(
[property: CliOption("--observability-configuration-name")] string ObservabilityConfigurationName
) : AwsOptions
{
    [CliOption("--trace-configuration")]
    public string? TraceConfiguration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}