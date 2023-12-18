using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "tenant-backfill", "start")]
public record AzAccountManagementGroupTenantBackfillStartOptions : AzOptions;