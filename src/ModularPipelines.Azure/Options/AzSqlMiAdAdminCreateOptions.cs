using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "mi", "ad-admin", "create")]
public record AzSqlMiAdAdminCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--managed-instance")] string ManagedInstance,
[property: CliOption("--object-id")] string ObjectId,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;