using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appflow", "describe-connector")]
public record AwsAppflowDescribeConnectorOptions(
[property: CommandSwitch("--connector-type")] string ConnectorType
) : AwsOptions
{
    [CommandSwitch("--connector-label")]
    public string? ConnectorLabel { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}