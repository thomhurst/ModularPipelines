using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "management-group", "check-name-availability")]
public record AzAccountManagementGroupCheckNameAvailabilityOptions(
[property: CliOption("--name")] string Name
) : AzOptions;