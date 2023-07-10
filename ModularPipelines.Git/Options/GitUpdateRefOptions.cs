using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("update-ref")]
public record GitUpdateRefOptions : GitOptions
{
}