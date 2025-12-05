using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "query-metadata")]
public record GcloudMetastoreServicesQueryMetadataOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliOption("--query")] string Query
) : GcloudOptions;