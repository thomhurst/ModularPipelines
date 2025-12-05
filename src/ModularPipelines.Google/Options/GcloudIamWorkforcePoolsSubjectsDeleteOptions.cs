using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "subjects", "delete")]
public record GcloudIamWorkforcePoolsSubjectsDeleteOptions(
[property: CliArgument] string Subject,
[property: CliArgument] string Location,
[property: CliArgument] string WorkforcePool
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}