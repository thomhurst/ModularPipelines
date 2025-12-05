using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "describe")]
public record GcloudMetastoreServicesDescribeOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location
) : GcloudOptions;