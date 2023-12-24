using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "update-endpoint")]
public record AwsComprehendUpdateEndpointOptions(
[property: CommandSwitch("--endpoint-arn")] string EndpointArn
) : AwsOptions
{
    [CommandSwitch("--desired-model-arn")]
    public string? DesiredModelArn { get; set; }

    [CommandSwitch("--desired-inference-units")]
    public int? DesiredInferenceUnits { get; set; }

    [CommandSwitch("--desired-data-access-role-arn")]
    public string? DesiredDataAccessRoleArn { get; set; }

    [CommandSwitch("--flywheel-arn")]
    public string? FlywheelArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}