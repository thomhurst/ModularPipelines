using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "list")]
public record GcloudIamWorkforcePoolsListOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions
{
    [BooleanCommandSwitch("--show-deleted")]
    public bool? ShowDeleted { get; set; }
}