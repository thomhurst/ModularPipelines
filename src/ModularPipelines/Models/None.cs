namespace ModularPipelines.Models;

/// <summary>
/// Represents the absence of a value. Used as the result type for modules that don't return meaningful data.
/// </summary>
/// <remarks>
/// This is similar to <c>void</c> but can be used as a generic type parameter.
/// Use <see cref="Modules.Module"/> (non-generic) instead of <see cref="Modules.Module{T}"/> with <see cref="None"/>
/// for modules that perform actions without returning results.
/// </remarks>
public readonly struct None : IEquatable<None>, IEquatable<None?>
{
    /// <summary>
    /// Gets the singleton value of <see cref="None"/>.
    /// </summary>
    public static None Value => default;

    /// <inheritdoc />
    public bool Equals(None other) => true;

    /// <inheritdoc />
    public bool Equals(None? other) => true; // None is semantically equivalent to null

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is None or null;

    /// <inheritdoc />
    public override int GetHashCode() => 0;

    /// <inheritdoc />
    public override string ToString() => "None";

    /// <summary>
    /// Determines whether two <see cref="None"/> values are equal (always true).
    /// </summary>
    public static bool operator ==(None left, None right) => true;

    /// <summary>
    /// Determines whether two <see cref="None"/> values are not equal (always false).
    /// </summary>
    public static bool operator !=(None left, None right) => false;

    /// <summary>
    /// Determines whether a <see cref="None"/> equals a nullable <see cref="None"/> (always true).
    /// </summary>
    public static bool operator ==(None left, None? right) => true;

    /// <summary>
    /// Determines whether a <see cref="None"/> does not equal a nullable <see cref="None"/> (always false).
    /// </summary>
    public static bool operator !=(None left, None? right) => false;

    /// <summary>
    /// Determines whether a nullable <see cref="None"/> equals a <see cref="None"/> (always true).
    /// </summary>
    public static bool operator ==(None? left, None right) => true;

    /// <summary>
    /// Determines whether a nullable <see cref="None"/> does not equal a <see cref="None"/> (always false).
    /// </summary>
    public static bool operator !=(None? left, None right) => false;

}
