using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "create-observability-configuration")]
public record AwsApprunnerCreateObservabilityConfigurationOptions(
[property: CommandSwitch("--observability-configuration-name")] string ObservabilityConfigurationName
) : AwsOptions
{
    [CommandSwitch("--trace-configuration")]
    public string? TraceConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}