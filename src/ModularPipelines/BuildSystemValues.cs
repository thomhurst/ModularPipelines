namespace ModularPipelines;

/// <summary>
/// Static helper methods for build system logging commands.
/// </summary>
/// <remarks>
/// This class is obsolete and will be removed in a future version.
/// Use <see cref="Engine.IBuildSystemFormatter"/> and <see cref="Engine.IBuildSystemFormatterProvider"/> instead.
/// </remarks>
[Obsolete("Use IBuildSystemFormatter and IBuildSystemFormatterProvider instead. This class will be removed in a future version.")]
public static class BuildSystemValues
{
    /// <summary>
    /// TeamCity service messages for log blocks.
    /// </summary>
    public static class TeamCity
    {
        /// <summary>
        /// Creates a TeamCity block opened service message.
        /// </summary>
        public static string StartBlock(string name) => $"##teamcity[blockOpened name='{name}']";

        /// <summary>
        /// Creates a TeamCity block closed service message.
        /// </summary>
        public static string EndBlock(string name) => $"##teamcity[blockClosed name='{name}']";
    }

    /// <summary>
    /// Azure Pipelines logging commands for log groups.
    /// </summary>
    public static class AzurePipelines
    {
        /// <summary>
        /// Creates an Azure Pipelines group start command.
        /// </summary>
        public static string StartBlock(string name) => $"##[group]{name}";

        /// <summary>
        /// Gets the Azure Pipelines group end command.
        /// </summary>
        public static string EndBlock => "##[endgroup]";
    }

    /// <summary>
    /// GitHub Actions workflow commands for log groups.
    /// </summary>
    public static class GitHub
    {
        /// <summary>
        /// Creates a GitHub Actions group start command.
        /// </summary>
        public static string StartBlock(string name) => $"::group::{name}";

        /// <summary>
        /// Gets the GitHub Actions group end command.
        /// </summary>
        public static string EndBlock => "::endgroup::";
    }
}