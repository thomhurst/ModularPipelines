using System.Collections;

namespace ModularPipelines.Models;

public class KeyValueVariables : IDictionary<string, string>
{
    public string Separator { get; set; }

    private readonly Dictionary<string, string> _innerDictionary = new();

    public KeyValueVariables() : this("=")
    {
    }

    public KeyValueVariables(string separator)
    {
        Separator = separator;
    }

    public static implicit operator KeyValueVariables(Dictionary<string, string> dictionary)
    {
        return FromDictionary(dictionary);
    }

    public static KeyValueVariables FromDictionary(IDictionary<string, string> dictionary, string separator = "=")
    {
        var keyValueVariables = new KeyValueVariables(separator);

        foreach (var (key, value) in dictionary)
        {
            keyValueVariables.Add(key, value);
        }

        return keyValueVariables;
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _innerDictionary.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _innerDictionary.GetEnumerator();
    }

    /// <inheritdoc/>
    public void Add(KeyValuePair<string, string> item)
    {
        _innerDictionary.Add(item.Key, item.Value);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _innerDictionary.Clear();
    }

    /// <inheritdoc/>
    public bool Contains(KeyValuePair<string, string> item)
    {
        return _innerDictionary.Contains(item);
    }

    /// <inheritdoc/>
    public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
    {
        (_innerDictionary as ICollection).CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    public bool Remove(KeyValuePair<string, string> item)
    {
        return _innerDictionary.Remove(item.Key);
    }

    /// <inheritdoc/>
    public int Count => _innerDictionary.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <inheritdoc/>
    public void Add(string key, string value)
    {
        _innerDictionary.Add(key, value);
    }

    /// <inheritdoc/>
    public bool ContainsKey(string key)
    {
        return _innerDictionary.ContainsKey(key);
    }

    /// <inheritdoc/>
    public bool Remove(string key)
    {
        return _innerDictionary.Remove(key);
    }

    /// <inheritdoc/>
    public bool TryGetValue(string key, out string value)
    {
        return _innerDictionary.TryGetValue(key, out value!);
    }

    /// <inheritdoc/>
    public string this[string key]
    {
        get => _innerDictionary[key];
        set => _innerDictionary[key] = value;
    }

    /// <inheritdoc/>
    public ICollection<string> Keys => _innerDictionary.Keys;

    /// <inheritdoc/>
    public ICollection<string> Values => _innerDictionary.Values;
}