using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "update-alias")]
public record AwsLambdaUpdateAliasOptions(
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--function-version")]
    public string? FunctionVersion { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--routing-config")]
    public string? RoutingConfig { get; set; }

    [CommandSwitch("--revision-id")]
    public string? RevisionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}