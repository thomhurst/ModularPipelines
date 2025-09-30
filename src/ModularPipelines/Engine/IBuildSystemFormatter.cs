namespace ModularPipelines.Engine;

/// <summary>
/// Defines formatting behavior for a specific CI/CD build system.
/// Provides collapsible log groups and secret masking commands specific to each build system.
/// </summary>
/// <example>
/// <code>
/// // Get formatter for current build system
/// var formatter = formatterProvider.GetFormatter(buildSystem);
///
/// // Create collapsible log group
/// var startCommand = formatter.GetStartBlockCommand("Build Step");
/// if (startCommand != null)
/// {
///     console.WriteLine(startCommand);
/// }
///
/// // Mask a secret
/// var maskCommand = formatter.GetMaskSecretCommand("my-secret-value");
/// if (maskCommand != null)
/// {
///     console.WriteLine(maskCommand);
/// }
/// </code>
/// </example>
internal interface IBuildSystemFormatter
{
    /// <summary>
    /// Gets the command to start a collapsible log block/group.
    /// </summary>
    /// <param name="name">The name of the log block.</param>
    /// <returns>The formatted command string, or null if not supported by this build system.</returns>
    string? GetStartBlockCommand(string name);

    /// <summary>
    /// Gets the command to end a collapsible log block/group.
    /// </summary>
    /// <param name="name">The name of the log block.</param>
    /// <returns>The formatted command string, or null if not supported by this build system.</returns>
    string? GetEndBlockCommand(string name);

    /// <summary>
    /// Gets the command to mask/obfuscate a secret value in logs.
    /// </summary>
    /// <param name="secret">The secret value to mask.</param>
    /// <returns>The formatted masking command, or null if not supported by this build system.</returns>
    string? GetMaskSecretCommand(string secret);
}