using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tag-templates", "describe")]
public record GcloudDataCatalogTagTemplatesDescribeOptions(
[property: CliArgument] string TagTemplate,
[property: CliArgument] string Location
) : GcloudOptions;