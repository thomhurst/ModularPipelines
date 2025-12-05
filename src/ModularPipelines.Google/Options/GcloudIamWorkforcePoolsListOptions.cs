using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "list")]
public record GcloudIamWorkforcePoolsListOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--organization")] string Organization
) : GcloudOptions
{
    [CliFlag("--show-deleted")]
    public bool? ShowDeleted { get; set; }
}