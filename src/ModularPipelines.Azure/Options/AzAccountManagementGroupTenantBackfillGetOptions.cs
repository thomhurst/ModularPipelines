using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "tenant-backfill", "get")]
public record AzAccountManagementGroupTenantBackfillGetOptions : AzOptions;