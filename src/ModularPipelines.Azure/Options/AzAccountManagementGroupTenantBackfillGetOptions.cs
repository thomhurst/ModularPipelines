using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "management-group", "tenant-backfill", "get")]
public record AzAccountManagementGroupTenantBackfillGetOptions : AzOptions;