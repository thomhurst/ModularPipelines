using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack", "mg", "list")]
public record AzStackMgListOptions(
[property: CliOption("--management-group-id")] string ManagementGroupId
) : AzOptions;