using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrassv2", "create-component-version")]
public record AwsGreengrassv2CreateComponentVersionOptions : AwsOptions
{
    [CliOption("--inline-recipe")]
    public string? InlineRecipe { get; set; }

    [CliOption("--lambda-function")]
    public string? LambdaFunction { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}