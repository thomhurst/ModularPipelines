using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("update-ref")]
[ExcludeFromCodeCoverage]
public record GitUpdateRefOptions : GitOptions;