using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("lab", "custom-image", "create")]
public record AzLabCustomImageCreateOptions(
[property: CliOption("--lab-name")] string LabName,
[property: CliOption("--name")] string Name,
[property: CliOption("--os-state")] string OsState,
[property: CliOption("--os-type")] string OsType,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source-vm-id")] string SourceVmId
) : AzOptions
{
    [CliOption("--author")]
    public string? Author { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }
}