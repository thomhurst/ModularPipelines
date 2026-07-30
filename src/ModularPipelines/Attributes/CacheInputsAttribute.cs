namespace ModularPipelines.Attributes;

/// <summary>
/// Declares files whose contents determine whether a module's cached result is reusable.
/// </summary>
/// <remarks>
/// Patterns are evaluated relative to <see cref="Caching.ModuleCacheOptions.WorkingDirectory"/>.
/// Standard <c>*</c>, <c>?</c>, and recursive <c>**</c> glob wildcards are supported.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class CacheInputsAttribute : Attribute
{
    /// <summary>
    /// Gets the input glob patterns.
    /// </summary>
    public IReadOnlyList<string> Patterns { get; }

    /// <summary>
    /// Initialises a new instance of the <see cref="CacheInputsAttribute"/> class.
    /// Initializes a new instance of the <see cref="CacheInputsAttribute"/> class.
    /// </summary>
    /// <param name="patterns">One or more input file paths or glob patterns.</param>
    public CacheInputsAttribute(params string[] patterns)
    {
        ArgumentNullException.ThrowIfNull(patterns);

        if (patterns.Length == 0 || patterns.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("At least one non-empty cache input pattern is required.", nameof(patterns));
        }

        Patterns = [.. patterns];
    }
}
