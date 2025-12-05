using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "invoke")]
public record AwsLambdaInvokeOptions(
[property: CliOption("--function-name")] string FunctionName
) : AwsOptions
{
    [CliOption("--invocation-type")]
    public string? InvocationType { get; set; }

    [CliOption("--log-type")]
    public string? LogType { get; set; }

    [CliOption("--client-context")]
    public string? ClientContext { get; set; }

    [CliOption("--payload")]
    public string? Payload { get; set; }

    [CliOption("--qualifier")]
    public string? Qualifier { get; set; }
}