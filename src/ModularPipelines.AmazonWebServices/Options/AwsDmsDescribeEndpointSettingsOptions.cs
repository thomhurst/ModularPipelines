using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "describe-endpoint-settings")]
public record AwsDmsDescribeEndpointSettingsOptions(
[property: CommandSwitch("--engine-name")] string EngineName
) : AwsOptions
{
    [CommandSwitch("--max-records")]
    public int? MaxRecords { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}