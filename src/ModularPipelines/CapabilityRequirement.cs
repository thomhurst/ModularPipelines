using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines;

/// <summary>
/// Describes the capabilities a process needs to execute a module.
/// </summary>
/// <remarks>
/// A requirement is a list of clauses. A process satisfies the requirement when it satisfies every
/// clause, and it satisfies a clause when it advertises at least one capability in that clause.
/// For example, <c>[[linux, macos], [docker]]</c> means "(linux or macos) and docker".
/// </remarks>
[JsonConverter(typeof(CapabilityRequirementJsonConverter))]
public sealed class CapabilityRequirement : IEquatable<CapabilityRequirement>
{
    private CapabilityRequirement(IReadOnlyList<IReadOnlyList<Capability>> clauses)
    {
        Clauses = clauses;
    }

    /// <summary>
    /// Gets a requirement that every process satisfies.
    /// </summary>
    public static CapabilityRequirement None { get; } = new([]);

    /// <summary>
    /// Gets the normalized clauses. Every clause must be satisfied by at least one of its capabilities.
    /// </summary>
    public IReadOnlyList<IReadOnlyList<Capability>> Clauses { get; }

    /// <summary>
    /// Gets whether this requirement has no clauses.
    /// </summary>
    public bool IsEmpty => Clauses.Count == 0;

    /// <summary>
    /// Gets whether some worker could satisfy this requirement. A worker runs exactly one
    /// operating system, so clauses that only list operating systems must have one in common.
    /// </summary>
    internal bool IsSatisfiable
    {
        get
        {
            HashSet<Capability>? operatingSystems = null;
            foreach (var clause in Clauses.Where(static clause => clause.All(static capability => capability.IsOperatingSystem)))
            {
                if (operatingSystems is null)
                {
                    operatingSystems = [.. clause];
                }
                else
                {
                    operatingSystems.IntersectWith(clause);
                }
            }

            return operatingSystems is not { Count: 0 };
        }
    }

    /// <summary>
    /// Creates a requirement that needs every specified capability.
    /// </summary>
    public static CapabilityRequirement AllOf(params Capability[] capabilities)
    {
        ArgumentNullException.ThrowIfNull(capabilities);
        return Create(capabilities.Select(static capability => new[] { capability }));
    }

    /// <summary>
    /// Creates a requirement that needs at least one of the specified capabilities.
    /// </summary>
    public static CapabilityRequirement AnyOf(params Capability[] capabilities)
    {
        ArgumentNullException.ThrowIfNull(capabilities);
        return Create([capabilities]);
    }

    /// <summary>
    /// Creates a requirement from clauses. Every clause must contain at least one capability.
    /// </summary>
    public static CapabilityRequirement Create(IEnumerable<IEnumerable<Capability>> clauses)
    {
        ArgumentNullException.ThrowIfNull(clauses);

        var normalizedClauses = new List<Capability[]>();
        foreach (var clause in clauses)
        {
            ArgumentNullException.ThrowIfNull(clause, nameof(clauses));
            var capabilities = clause.Distinct().ToArray();
            if (capabilities.Length == 0)
            {
                throw new ArgumentException("A capability clause must contain at least one capability.", nameof(clauses));
            }

            if (capabilities.Any(static capability => string.IsNullOrWhiteSpace(capability.Name)))
            {
                throw new ArgumentException("A capability name cannot be empty.", nameof(clauses));
            }

            Array.Sort(capabilities, static (left, right) => CompareNames(left.Name, right.Name));
            normalizedClauses.Add(capabilities);
        }

        // Compare clauses by their capabilities, not their display text: custom names may contain " | ".
        // A clause that contains a smaller clause adds no constraint: drop it.
        var distinctClauses = normalizedClauses
            .Distinct(ClauseComparer.Instance)
            .ToArray();
        var minimalClauses = distinctClauses
            .Where(clause => !distinctClauses.Any(other => other.Length < clause.Length && IsSubset(other, clause)))
            .Order(ClauseComparer.Instance)
            .Select(static clause => (IReadOnlyList<Capability>) Array.AsReadOnly(clause))
            .ToArray();

        return minimalClauses.Length == 0 ? None : new CapabilityRequirement(minimalClauses);
    }

    /// <summary>
    /// Returns a requirement that needs both this requirement and <paramref name="other"/>.
    /// </summary>
    public CapabilityRequirement And(CapabilityRequirement other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return other.IsEmpty ? this : IsEmpty ? other : Create(Clauses.Concat(other.Clauses));
    }

    /// <summary>
    /// Returns a requirement that needs either this requirement or <paramref name="other"/>.
    /// </summary>
    public CapabilityRequirement Or(CapabilityRequirement other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (IsEmpty || other.IsEmpty)
        {
            return None;
        }

        return Create(
            from left in Clauses
            from right in other.Clauses
            select left.Concat(right));
    }

    /// <summary>
    /// Returns whether a worker advertising <paramref name="capabilities"/> satisfies this requirement.
    /// </summary>
    public bool IsSatisfiedBy(IEnumerable<Capability> capabilities)
    {
        ArgumentNullException.ThrowIfNull(capabilities);
        if (IsEmpty)
        {
            return true;
        }

        var available = capabilities as IReadOnlySet<Capability> ?? capabilities.ToHashSet();
        return Clauses.All(clause => clause.Any(available.Contains));
    }

    /// <inheritdoc />
    public bool Equals(CapabilityRequirement? other) =>
        other is not null
        && Clauses.Count == other.Clauses.Count
        && Clauses.Zip(other.Clauses).All(static pair => pair.First.SequenceEqual(pair.Second));

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is CapabilityRequirement other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = default(HashCode);
        foreach (var clause in Clauses)
        {
            hash.Add(clause.Count);
            foreach (var capability in clause)
            {
                hash.Add(capability);
            }
        }

        return hash.ToHashCode();
    }

    /// <inheritdoc />
    public override string ToString() =>
        IsEmpty
            ? "none"
            : string.Join(" & ", Clauses.Select(static clause =>
                clause.Count == 1 ? clause[0].Name : $"({Format(clause)})"));

    private static string Format(IEnumerable<Capability> clause) =>
        string.Join(" | ", clause.Select(static capability => capability.Name));

    private static bool IsSubset(IReadOnlyCollection<Capability> candidate, IReadOnlyCollection<Capability> clause) =>
        candidate.Count <= clause.Count && candidate.All(clause.Contains);

    private sealed class ClauseComparer : IComparer<Capability[]>, IEqualityComparer<Capability[]>
    {
        public static ClauseComparer Instance { get; } = new();

        public int Compare(Capability[]? left, Capability[]? right)
        {
            if (ReferenceEquals(left, right))
            {
                return 0;
            }

            if (left is null || right is null)
            {
                return left is null ? -1 : 1;
            }

            var result = left.Length.CompareTo(right.Length);
            for (var index = 0; result == 0 && index < left.Length; index++)
            {
                result = StringComparer.OrdinalIgnoreCase.Compare(left[index].Name, right[index].Name);
            }

            return result;
        }

        public bool Equals(Capability[]? left, Capability[]? right) => Compare(left, right) == 0;

        public int GetHashCode(Capability[] clause)
        {
            var hash = default(HashCode);
            foreach (var capability in clause)
            {
                hash.Add(capability);
            }

            return hash.ToHashCode();
        }
    }

    private static int CompareNames(string left, string right)
    {
        var result = StringComparer.OrdinalIgnoreCase.Compare(left, right);
        return result != 0 ? result : StringComparer.Ordinal.Compare(left, right);
    }
}

internal sealed class CapabilityRequirementJsonConverter : JsonConverter<CapabilityRequirement>
{
    public override CapabilityRequirement Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected a capability requirement array.");
        }

        var clauses = new List<List<Capability>>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected a capability clause array.");
            }

            var clause = new List<Capability>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                clause.Add(reader.TokenType == JsonTokenType.String
                    ? new Capability(reader.GetString()!)
                    : throw new JsonException("Expected a capability string."));
            }

            clauses.Add(clause);
        }

        try
        {
            return CapabilityRequirement.Create(clauses);
        }
        catch (ArgumentException exception)
        {
            throw new JsonException(exception.Message, exception);
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        CapabilityRequirement value,
        JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var clause in value.Clauses)
        {
            writer.WriteStartArray();
            foreach (var capability in clause)
            {
                CapabilityJsonConverter.WriteValue(writer, capability);
            }

            writer.WriteEndArray();
        }

        writer.WriteEndArray();
    }
}
