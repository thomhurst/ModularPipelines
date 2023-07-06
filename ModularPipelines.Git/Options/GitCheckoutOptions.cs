using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("checkout")]
public record GitCheckoutOptions(string BranchName) : GitCommandOptions;

[CommandPrecedingArguments("push", "--set-upstream", "origin")]
public record GitSetUpstreamOptions(string BranchName) : GitCommandOptions;
