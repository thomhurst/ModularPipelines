using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "management-group", "tenant-backfill", "get")]
public record AzAccountManagementGroupTenantBackfillGetOptions : AzOptions;