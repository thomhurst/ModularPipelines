using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "update-function-url-config")]
public record AwsLambdaUpdateFunctionUrlConfigOptions(
[property: CliOption("--function-name")] string FunctionName
) : AwsOptions
{
    [CliOption("--qualifier")]
    public string? Qualifier { get; set; }

    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--cors")]
    public string? Cors { get; set; }

    [CliOption("--invoke-mode")]
    public string? InvokeMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}