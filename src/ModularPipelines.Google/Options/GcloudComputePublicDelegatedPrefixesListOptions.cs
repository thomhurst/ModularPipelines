using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "public-delegated-prefixes", "list")]
public record GcloudComputePublicDelegatedPrefixesListOptions : GcloudOptions;