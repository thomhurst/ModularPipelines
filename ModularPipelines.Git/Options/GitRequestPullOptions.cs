using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("request-pull")]
public record GitRequestPullOptions : GitOptions
{
}