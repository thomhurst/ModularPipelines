using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "put-external-model")]
public record AwsFrauddetectorPutExternalModelOptions(
[property: CommandSwitch("--model-endpoint")] string ModelEndpoint,
[property: CommandSwitch("--model-source")] string ModelSource,
[property: CommandSwitch("--invoke-model-endpoint-role-arn")] string InvokeModelEndpointRoleArn,
[property: CommandSwitch("--input-configuration")] string InputConfiguration,
[property: CommandSwitch("--output-configuration")] string OutputConfiguration,
[property: CommandSwitch("--model-endpoint-status")] string ModelEndpointStatus
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}