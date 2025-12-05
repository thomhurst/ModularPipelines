using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "update-alias")]
public record AwsLambdaUpdateAliasOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--function-version")]
    public string? FunctionVersion { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--routing-config")]
    public string? RoutingConfig { get; set; }

    [CliOption("--revision-id")]
    public string? RevisionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}