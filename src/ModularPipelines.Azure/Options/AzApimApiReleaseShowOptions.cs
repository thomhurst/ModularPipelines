using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "api", "release", "show")]
public record AzApimApiReleaseShowOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--release-id")] string ReleaseId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;