using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "create-alias")]
public record AwsLambdaCreateAliasOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--name")] string Name,
[property: CliOption("--function-version")] string FunctionVersion
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--routing-config")]
    public string? RoutingConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}