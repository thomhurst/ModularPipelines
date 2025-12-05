using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "create-function-url-config")]
public record AwsLambdaCreateFunctionUrlConfigOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--auth-type")] string AuthType
) : AwsOptions
{
    [CliOption("--qualifier")]
    public string? Qualifier { get; set; }

    [CliOption("--cors")]
    public string? Cors { get; set; }

    [CliOption("--invoke-mode")]
    public string? InvokeMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}