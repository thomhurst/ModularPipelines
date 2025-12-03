using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "sql", "pool", "classification", "create")]
public record AzSynapseSqlPoolClassificationCreateOptions(
[property: CliOption("--column")] string Column,
[property: CliOption("--information-type")] string InformationType,
[property: CliOption("--label")] string Label,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--schema")] string Schema,
[property: CliOption("--table")] string Table,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;