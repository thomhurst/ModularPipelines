using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "query-metadata")]
public record GcloudMetastoreServicesQueryMetadataOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--query")] string Query
) : GcloudOptions;