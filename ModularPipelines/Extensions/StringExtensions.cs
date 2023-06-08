namespace ModularPipelines.Extensions;

public static class StringExtensions
{
    public static TCollection AddNonNullOrEmpty<TCollection>(this TCollection collection, string? argument) where TCollection : ICollection<string>
    {
        if (!string.IsNullOrEmpty(argument))
        {
            collection.Add(argument);
        }

        return collection;
    }
    
    public static TCollection AddRangeNonNullOrEmpty<TCollection>(this TCollection collection, IEnumerable<string>? arguments) where TCollection : ICollection<string>
    {
        foreach (var argument in arguments ?? Array.Empty<string>())
        {
            collection.AddNonNullOrEmpty(argument);
        }

        return collection;
    }
    
    public static TCollection AddNonNullOrEmptyArgumentWithPrefix<TCollection>(this TCollection collection, string argumentPrefix, string? argument) where TCollection : ICollection<string>
    {
        if (!string.IsNullOrEmpty(argument))
        {
            collection.Add($"{argumentPrefix}{argument}");
        }

        return collection;
    }
    
    public static TCollection AddRangeNonNullOrEmptyArgumentWithPrefix<TCollection>(this TCollection collection, string argumentPrefix, IEnumerable<string>? arguments) where TCollection : ICollection<string>
    {
        foreach (var argument in arguments ?? Array.Empty<string>())
        {
            collection.AddNonNullOrEmptyArgumentWithPrefix(argumentPrefix, argument);
        }

        return collection;
    } 
    
    public static TCollection AddNonNullOrEmptyArgumentWithSwitch<TCollection>(this TCollection collection, string argumentSwitch, string? argument) where TCollection : ICollection<string>
    {
        if (!string.IsNullOrEmpty(argument))
        {
            collection.Add(argumentSwitch);
            collection.Add(argument);
        }

        return collection;
    }
    
    public static TCollection AddRangeNonNullOrEmptyArgumentWithSwitch<TCollection>(this TCollection collection, string argumentSwitch, IEnumerable<string>? arguments) where TCollection : ICollection<string>
    {
        foreach (var argument in arguments ?? Array.Empty<string>())
        {
            collection.AddNonNullOrEmptyArgumentWithSwitch(argumentSwitch, argument);
        }

        return collection;
    } 
}