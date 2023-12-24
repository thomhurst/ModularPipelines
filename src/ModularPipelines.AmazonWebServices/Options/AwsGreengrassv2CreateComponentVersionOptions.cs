using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrassv2", "create-component-version")]
public record AwsGreengrassv2CreateComponentVersionOptions : AwsOptions
{
    [CommandSwitch("--inline-recipe")]
    public string? InlineRecipe { get; set; }

    [CommandSwitch("--lambda-function")]
    public string? LambdaFunction { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}