namespace ModularPipelines.Secrets;

internal interface ISecretProvider
{
    /// <summary>
    /// Gets the version of the registered secret collection.
    /// </summary>
    long Version { get; }

    /// <summary>
    /// Gets a list of the detected secrets from IOptions objects.
    /// </summary>
    IEnumerable<string> Secrets { get; }

    /// <summary>
    /// Gets an atomic snapshot of the registered secrets and their version.
    /// </summary>
    SecretSnapshot GetSnapshot();

    /// <summary>
    /// Executes an action while preventing secret registration, provided the
    /// registered-secret version is still current.
    /// </summary>
    bool TryExecuteIfVersionCurrent(long expectedVersion, Action action);

    /// <summary>
    /// Returns any values in the object marked with the [SecretValue] attribute.
    /// </summary>
    /// <param name="value">Object to check for secret values within its properties.</param>
    /// <returns>Array of secrets.</returns>
    IEnumerable<string> GetSecretsInObject(object? value);
}

internal interface ISecretEmissionGuard
{
    /// <summary>
    /// Executes output processing against a stable registered-secret collection.
    /// Concurrent output may proceed together, while secret publication waits for it to finish.
    /// Registrations made reentrantly by the output sink are published when the callback completes.
    /// </summary>
    /// <typeparam name="TState">The state passed to the output callback.</typeparam>
    /// <param name="state">The output state.</param>
    /// <param name="processOutput">The output processing and emission to execute.</param>
    void ExecuteWithStableSecrets<TState>(TState state, Action<TState> processOutput);
}

internal readonly record struct SecretSnapshot(long Version, IReadOnlyList<string> Secrets);
