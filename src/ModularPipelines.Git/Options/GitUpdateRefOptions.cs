using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("update-ref")]
[ExcludeFromCodeCoverage]
public record GitUpdateRefOptions : GitOptions;