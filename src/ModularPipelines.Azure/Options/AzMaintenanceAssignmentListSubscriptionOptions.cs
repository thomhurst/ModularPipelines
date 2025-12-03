using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("maintenance", "assignment", "list-subscription")]
public record AzMaintenanceAssignmentListSubscriptionOptions : AzOptions;