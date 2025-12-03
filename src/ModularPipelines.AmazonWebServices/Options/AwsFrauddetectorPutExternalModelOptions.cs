using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "put-external-model")]
public record AwsFrauddetectorPutExternalModelOptions(
[property: CliOption("--model-endpoint")] string ModelEndpoint,
[property: CliOption("--model-source")] string ModelSource,
[property: CliOption("--invoke-model-endpoint-role-arn")] string InvokeModelEndpointRoleArn,
[property: CliOption("--input-configuration")] string InputConfiguration,
[property: CliOption("--output-configuration")] string OutputConfiguration,
[property: CliOption("--model-endpoint-status")] string ModelEndpointStatus
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}