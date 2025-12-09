using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "tail")]
public record AwsLogsTailOptions : AwsOptions
{
    [CliOption("--since")]
    public string? Since { get; set; }

    [CliFlag("--follow")]
    public bool? Follow { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--filter-pattern")]
    public string? FilterPattern { get; set; }

    [CliOption("--log-stream-names")]
    public string? LogStreamNames { get; set; }

    [CliOption("--log-stream-name-prefix")]
    public string? LogStreamNamePrefix { get; set; }
}