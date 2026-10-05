namespace ModularPipelines.Models;

/// <summary>
/// Represents the consecutive values belonging to one occurrence of a command-line option.
/// An empty group renders a bare option only when the option accepts optional values.
/// The option must enable <see cref="Attributes.CliOptionAttribute.GroupValues"/> and use space-separated values.
/// </summary>
public sealed class CliValueGroup
{
    /// <summary>
    /// Initialises a new instance of the <see cref="CliValueGroup"/> class.
    /// Copies the values so later collection changes cannot alter the group.
    /// </summary>
    /// <param name="values">The values for one option occurrence. Null values are not allowed.</param>
    public CliValueGroup(IEnumerable<string> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        var snapshot = values.ToArray();
        if (snapshot.Any(static value => value is null))
        {
            throw new ArgumentException("CLI value groups cannot contain null values.", nameof(values));
        }

        Values = Array.AsReadOnly(snapshot);
    }

    /// <summary>
    /// Gets the values for this option occurrence in their original order.
    /// </summary>
    public IReadOnlyList<string> Values { get; }
}
