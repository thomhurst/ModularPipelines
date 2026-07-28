using System.Collections;
using System.Collections.Immutable;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// An immutable array whose equality is based on its elements.
/// </summary>
internal readonly struct EquatableArray<T> : IReadOnlyList<T>, IEquatable<EquatableArray<T>>
{
    private readonly ImmutableArray<T> _items;

    public EquatableArray(ImmutableArray<T> items)
    {
        _items = items.IsDefault ? ImmutableArray<T>.Empty : items;
    }

    public static EquatableArray<T> Empty { get; } = new(ImmutableArray<T>.Empty);

    public int Count => Items.Length;

    public bool IsEmpty => Items.IsEmpty;

    public T this[int index] => Items[index];

    private ImmutableArray<T> Items => _items.IsDefault ? ImmutableArray<T>.Empty : _items;

    public bool Equals(EquatableArray<T> other)
    {
        var items = Items;
        var otherItems = other.Items;
        if (items.Length != otherItems.Length)
        {
            return false;
        }

        var comparer = EqualityComparer<T>.Default;
        for (var i = 0; i < items.Length; i++)
        {
            if (!comparer.Equals(items[i], otherItems[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj) => obj is EquatableArray<T> other && Equals(other);

    public override int GetHashCode()
    {
        var hashCode = 17;
        var comparer = EqualityComparer<T>.Default;
        foreach (var item in Items)
        {
            hashCode = unchecked((hashCode * 31) + (item is null ? 0 : comparer.GetHashCode(item)));
        }

        return hashCode;
    }

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>) Items).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static implicit operator EquatableArray<T>(ImmutableArray<T> items) => new(items);

    public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right) => left.Equals(right);

    public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right) => !left.Equals(right);
}
