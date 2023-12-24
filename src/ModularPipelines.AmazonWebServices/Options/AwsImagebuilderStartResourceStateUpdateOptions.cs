using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "start-resource-state-update")]
public record AwsImagebuilderStartResourceStateUpdateOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--state")] string State
) : AwsOptions
{
    [CommandSwitch("--execution-role")]
    public string? ExecutionRole { get; set; }

    [CommandSwitch("--include-resources")]
    public string? IncludeResources { get; set; }

    [CommandSwitch("--exclusion-rules")]
    public string? ExclusionRules { get; set; }

    [CommandSwitch("--update-at")]
    public long? UpdateAt { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}