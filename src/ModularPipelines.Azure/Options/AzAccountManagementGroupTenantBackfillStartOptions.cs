using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "management-group", "tenant-backfill", "start")]
public record AzAccountManagementGroupTenantBackfillStartOptions : AzOptions;