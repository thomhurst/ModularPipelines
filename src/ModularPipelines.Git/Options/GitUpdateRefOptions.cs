using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("update-ref")]
[ExcludeFromCodeCoverage]
public record GitUpdateRefOptions : GitOptions;