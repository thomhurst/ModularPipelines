using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "management-group", "tenant-backfill", "start")]
public record AzAccountManagementGroupTenantBackfillStartOptions : AzOptions;