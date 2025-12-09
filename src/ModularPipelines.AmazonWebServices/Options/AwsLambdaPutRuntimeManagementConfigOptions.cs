using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "put-runtime-management-config")]
public record AwsLambdaPutRuntimeManagementConfigOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--update-runtime-on")] string UpdateRuntimeOn
) : AwsOptions
{
    [CliOption("--qualifier")]
    public string? Qualifier { get; set; }

    [CliOption("--runtime-version-arn")]
    public string? RuntimeVersionArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}