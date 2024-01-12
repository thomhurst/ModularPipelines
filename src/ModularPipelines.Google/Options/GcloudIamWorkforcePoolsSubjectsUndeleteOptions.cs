using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "subjects", "undelete")]
public record GcloudIamWorkforcePoolsSubjectsUndeleteOptions(
[property: PositionalArgument] string Subject,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string WorkforcePool
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}