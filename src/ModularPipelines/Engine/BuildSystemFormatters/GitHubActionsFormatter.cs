namespace ModularPipelines.Engine.BuildSystemFormatters;

/// <summary>
/// Formatter for GitHub Actions build system.
/// Supports collapsible log groups and secret masking via workflow commands.
/// </summary>
/// <remarks>
/// GitHub Actions uses workflow commands in the format ::command::value
/// Documentation: https://docs.github.com/en/actions/using-workflows/workflow-commands-for-github-actions
/// </remarks>
/// <example>
/// <code>
/// // Example output:
/// // ::group::Build Step
/// // ... log content ...
/// // ::endgroup::
/// //
/// // ::add-mask::my-secret-password
/// </code>
/// </example>
internal class GitHubActionsFormatter : IBuildSystemFormatter
{
    public string GetStartBlockCommand(string name) => $"::group::{name}";

    public string GetEndBlockCommand(string name) => "::endgroup::";

    public string GetMaskSecretCommand(string secret) => $"::add-mask::{secret}";
}
