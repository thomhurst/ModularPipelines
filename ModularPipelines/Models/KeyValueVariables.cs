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

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _innerDictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable) _innerDictionary).GetEnumerator();
    }

    public void Add(KeyValuePair<string, string> item)
    {
        _innerDictionary.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _innerDictionary.Clear();
    }

    public bool Contains(KeyValuePair<string, string> item)
    {
        return _innerDictionary.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
    {
        (_innerDictionary as ICollection).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<string, string> item)
    {
        return _innerDictionary.Remove(item.Key);
    }

    public int Count => _innerDictionary.Count;

    public bool IsReadOnly => false;

    public void Add(string key, string value)
    {
        _innerDictionary.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
        return _innerDictionary.ContainsKey(key);
    }

    public bool Remove(string key)
    {
        return _innerDictionary.Remove(key);
    }

    public bool TryGetValue(string key, out string value)
    {
        return _innerDictionary.TryGetValue(key, out value!);
    }

    public string this[string key]
    {
        get => _innerDictionary[key];
        set => _innerDictionary[key] = value;
    }

    public ICollection<string> Keys => ((IDictionary<string, string>) _innerDictionary).Keys;

    public ICollection<string> Values => ((IDictionary<string, string>) _innerDictionary).Values;
}