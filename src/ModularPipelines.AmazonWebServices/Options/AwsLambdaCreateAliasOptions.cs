using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "create-alias")]
public record AwsLambdaCreateAliasOptions(
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--function-version")] string FunctionVersion
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--routing-config")]
    public string? RoutingConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}