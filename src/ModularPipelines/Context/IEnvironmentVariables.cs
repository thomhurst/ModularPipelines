namespace ModularPipelines.Context;

/// <summary>
/// Provides functionality for reading and modifying environment variables.
/// This interface abstracts access to system environment variables, allowing retrieval
/// and modification of variables at different scopes (process, user, or machine level).
/// </summary>
public interface IEnvironmentVariables
{
    /// <summary>
    /// Gets the value of the specified environment variable.
    /// </summary>
    /// <param name="name">The name of the environment variable to retrieve.</param>
    /// <param name="target">
    /// The target scope from which to retrieve the environment variable.
    /// Defaults to <see cref="EnvironmentVariableTarget.Process"/>.
    /// </param>
    /// <returns>
    /// The value of the environment variable specified by <paramref name="name"/>,
    /// or <c>null</c> if the environment variable is not found.
    /// </returns>
    string? GetEnvironmentVariable(string name, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);

    /// <summary>
    /// Gets all environment variables as a dictionary.
    /// </summary>
    /// <param name="target">
    /// The target scope from which to retrieve environment variables.
    /// Defaults to <see cref="EnvironmentVariableTarget.Process"/>.
    /// </param>
    /// <returns>
    /// A dictionary containing all environment variable names and their values
    /// for the specified target scope.
    /// </returns>
    IDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);

    /// <summary>
    /// Sets the value of an environment variable.
    /// </summary>
    /// <param name="variableName">The name of the environment variable to set.</param>
    /// <param name="value">The value to assign to the environment variable.</param>
    /// <param name="target">
    /// The target scope in which to set the environment variable.
    /// Defaults to <see cref="EnvironmentVariableTarget.Process"/>.
    /// </param>
    void SetEnvironmentVariable(string variableName, string value, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);

    /// <summary>
    /// Gets the PATH environment variable as a list of individual paths.
    /// The PATH variable is split using the platform-appropriate delimiter
    /// (semicolon on Windows, colon on Unix-like systems).
    /// </summary>
    /// <param name="target">
    /// The target scope from which to retrieve the PATH variable.
    /// Defaults to <see cref="EnvironmentVariableTarget.Process"/>.
    /// </param>
    /// <returns>
    /// A read-only list of paths from the PATH environment variable.
    /// Returns an empty list if the PATH variable is not set.
    /// </returns>
    IReadOnlyList<string> GetPath(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds a path to the PATH environment variable.
    /// The path is appended using the platform-appropriate delimiter
    /// (semicolon on Windows, colon on Unix-like systems).
    /// </summary>
    /// <param name="pathToAdd">The path to add to the PATH environment variable.</param>
    /// <param name="target">
    /// The target scope in which to modify the PATH variable.
    /// Defaults to <see cref="EnvironmentVariableTarget.Process"/>.
    /// </param>
    void AddToPath(string pathToAdd, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);
}