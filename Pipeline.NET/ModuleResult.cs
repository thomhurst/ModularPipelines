namespace Pipeline.NET;

public class ModuleResult
{
    private const string DefaultKey = "__DEFAULT_ITEM__";
    private readonly Dictionary<string, object> _internalDictionary = new();

    public static ModuleResult From<T>(T t)
    {
        if (t == null)
        {
            return new ModuleResult();
        }
        
        if (t is IDictionary<string, object> dictionary)
        {
            return FromDictionary(dictionary);
        }
        
        var moduleResult = new ModuleResult();
        
        moduleResult._internalDictionary.Add(DefaultKey, t);

        return moduleResult;
    }
    
    public static ModuleResult FromDictionary(IDictionary<string, object> dictionary)
    {
        var moduleResult = new ModuleResult();
        
        foreach (var (key, value) in dictionary)
        {
            moduleResult._internalDictionary.Add(key, value);
        }

        return moduleResult;
    }

    public IDictionary<string, object> GetDictionary()
    {
        return _internalDictionary;
    }

    public T? Get<T>()
    {
        if (_internalDictionary.TryGetValue(DefaultKey, out var item))
        {
            return (T) item;
        }

        return default;
    }
}