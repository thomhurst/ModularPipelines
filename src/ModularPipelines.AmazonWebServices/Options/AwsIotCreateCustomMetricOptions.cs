using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-custom-metric")]
public record AwsIotCreateCustomMetricOptions(
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--metric-type")] string MetricType
) : AwsOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}