using ModularPipelines.Context;
using ModularPipelines.Git.Options;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Options;
using ModularPipelines.Secrets;

namespace ModularPipelines.Git;

internal static class GitCommandArguments
{
    private static readonly CommandModelProvider ModelProvider = new();
    private static readonly CommandArgumentBuilder ArgumentBuilder = new();
    private static readonly CommandPartsProvider PartsProvider = new();

    public static string[] Build(GitOptions options) =>
        [.. CommandLineBuilder.BuildArguments(
            options,
            PartsProvider.GetRawCommandParts(options),
            ModelProvider.GetCommandModel(options.GetType()),
            ArgumentBuilder)];

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
