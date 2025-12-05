using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "public-advertised-prefixes", "list")]
public record GcloudComputePublicAdvertisedPrefixesListOptions : GcloudOptions;