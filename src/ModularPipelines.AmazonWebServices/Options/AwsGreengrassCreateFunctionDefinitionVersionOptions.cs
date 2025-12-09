using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "create-function-definition-version")]
public record AwsGreengrassCreateFunctionDefinitionVersionOptions(
[property: CliOption("--function-definition-id")] string FunctionDefinitionId
) : AwsOptions
{
    [CliOption("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CliOption("--default-config")]
    public string? DefaultConfig { get; set; }

    [CliOption("--functions")]
    public string[]? Functions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}