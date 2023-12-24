using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipes", "list-pipes")]
public record AwsPipesListPipesOptions : AwsOptions
{
    [CommandSwitch("--current-state")]
    public string? CurrentState { get; set; }

    [CommandSwitch("--desired-state")]
    public string? DesiredState { get; set; }

    [CommandSwitch("--name-prefix")]
    public string? NamePrefix { get; set; }

    [CommandSwitch("--source-prefix")]
    public string? SourcePrefix { get; set; }

    [CommandSwitch("--target-prefix")]
    public string? TargetPrefix { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}