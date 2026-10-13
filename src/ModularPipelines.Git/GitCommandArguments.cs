using ModularPipelines.Git.Options;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Secrets;

namespace ModularPipelines.Git;

internal static class GitCommandArguments
{
    private static readonly CommandModelProvider ModelProvider = new();
    private static readonly CommandArgumentBuilder ArgumentBuilder = new();

    public static string[] Build(GitOptions options) =>
    [
        .. ArgumentBuilder.BuildArguments(ModelProvider.GetCommandModel(typeof(GitOptions)), options),
        .. options.CommandParts ?? [],
        .. options.Arguments ?? [],
    ];

    public static CommandExecutionOptions WithSecrets(
        GitOptions options, CommandExecutionOptions? executionOptions) =>
        new GitExecutionOptions(executionOptions ?? new CommandExecutionOptions())
        {
            Configuration = options.Configuration,
        };

    internal sealed record GitExecutionOptions : CommandExecutionOptions
    {
        public GitExecutionOptions(CommandExecutionOptions options)
            : base(options)
        {
        }

        [SecretValue]
        public KeyValue[]? Configuration { get; init; }
    }
}
