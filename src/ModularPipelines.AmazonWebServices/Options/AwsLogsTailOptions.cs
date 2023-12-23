using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "tail")]
public record AwsLogsTailOptions : AwsOptions
{
    [CommandSwitch("--since")]
    public string? Since { get; set; }

    [BooleanCommandSwitch("--follow")]
    public bool? Follow { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--filter-pattern")]
    public string? FilterPattern { get; set; }

    [CommandSwitch("--log-stream-names")]
    public string? LogStreamNames { get; set; }

    [CommandSwitch("--log-stream-name-prefix")]
    public string? LogStreamNamePrefix { get; set; }
}