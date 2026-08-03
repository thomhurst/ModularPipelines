using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace ModularPipelines.Engine;

internal static class ModuleTypeIdentifier
{
    private static readonly Lock LoadContextLock = new();
    private static readonly ConditionalWeakTable<Type, string> TypeIdentities = [];
    private static readonly ConditionalWeakTable<Type, string> RuntimeTypeIdentities = [];
    private static readonly ConditionalWeakTable<AssemblyLoadContext, Dictionary<string, LoadContextIdentity>> LoadContextIdentities = [];
    private static readonly Dictionary<string, List<ActiveLoadContextIdentity>> ActiveLoadContextIdentities = [with(StringComparer.Ordinal)];

    public static string Get(Type moduleType) =>
        TypeIdentities.GetValue(moduleType, CreateTypeIdentity);

    public static string GetRuntime(Type moduleType) =>
        RuntimeTypeIdentities.GetValue(moduleType, CreateRuntimeTypeIdentity);

    private static string CreateRuntimeTypeIdentity(Type type)
    {
        var identity = $"{Get(type)}\0RuntimeAssembly={type.Module.ModuleVersionId:N}";
        if (type.HasElementType)
        {
            return $"{identity}\0RuntimeElement={CreateRuntimeTypeIdentity(type.GetElementType()!)}";
        }

        if (!type.IsGenericType)
        {
            return identity;
        }

        var arguments = string.Join(
            "\u001F",
            type.GetGenericArguments().Select(CreateRuntimeTypeIdentity));
        return $"{identity}\0RuntimeArguments={arguments}";
    }

    private static string CreateTypeIdentity(Type moduleType)
    {
        var identifier = GetStableTypeIdentity(moduleType);
        var loadContextIdentity = GetLoadContextIdentity(moduleType, identifier);
        return loadContextIdentity is null
            ? identifier
            : $"{identifier}, LoadContext={loadContextIdentity}";
    }

    private static string GetStableTypeIdentity(Type type)
    {
        if (type.IsGenericParameter)
        {
            return $"!{type.GenericParameterPosition}:{type.Name}";
        }

        var typeName = GetStableTypeName(type);
        var assemblyIdentity = type.Assembly.GetName().Name;
        return string.IsNullOrWhiteSpace(assemblyIdentity)
            ? typeName
            : $"{typeName}, {assemblyIdentity}";
    }

    private static string GetStableTypeName(Type type)
    {
        if (type.IsArray)
        {
            var rank = type.GetArrayRank();
            var suffix = type.IsSZArray
                ? "[]"
                : rank == 1
                    ? "[*]"
                    : $"[{new string(',', rank - 1)}]";
            return GetStableTypeName(type.GetElementType()!) + suffix;
        }

        if (type.IsByRef)
        {
            return GetStableTypeName(type.GetElementType()!) + "&";
        }

        if (type.IsPointer)
        {
            return GetStableTypeName(type.GetElementType()!) + "*";
        }

        if (!type.IsGenericType)
        {
            return type.FullName ?? type.Name;
        }

        var definition = type.GetGenericTypeDefinition();
        var definitionName = definition.FullName ?? definition.Name;
        var arguments = string.Join(
            ", ",
            type.GetGenericArguments().Select(GetStableTypeIdentity));
        return $"{definitionName}[{arguments}]";
    }

    private static string? GetLoadContextIdentity(Type moduleType, string moduleTypeIdentity)
    {
        var loadContext = AssemblyLoadContext.GetLoadContext(moduleType.Assembly);
        if (loadContext is null || ReferenceEquals(loadContext, AssemblyLoadContext.Default))
        {
            return null;
        }

        lock (LoadContextLock)
        {
            var identitiesByModuleType = LoadContextIdentities.GetOrCreateValue(loadContext);
            if (identitiesByModuleType.TryGetValue(moduleTypeIdentity, out var existingIdentity))
            {
                return existingIdentity.Value;
            }

            var name = string.IsNullOrWhiteSpace(loadContext.Name)
                ? "unnamed"
                : loadContext.Name;
            var collisionKey = $"{name}\0{moduleTypeIdentity}";
            RemoveCollectedLoadContexts();
            if (!ActiveLoadContextIdentities.TryGetValue(collisionKey, out var activeIdentities))
            {
                activeIdentities = [];
                ActiveLoadContextIdentities[collisionKey] = activeIdentities;
            }

            var sequence = 1L;
            while (activeIdentities.Any(identity => identity.Sequence == sequence))
            {
                sequence++;
            }

            activeIdentities.Add(new ActiveLoadContextIdentity(
                sequence,
                new WeakReference<AssemblyLoadContext>(loadContext)));
            var identity = new LoadContextIdentity(name, sequence);
            identitiesByModuleType.Add(moduleTypeIdentity, identity);
            return identity.Value;
        }
    }

    private static void RemoveCollectedLoadContexts()
    {
        foreach (var (name, identities) in ActiveLoadContextIdentities.ToArray())
        {
            identities.RemoveAll(static identity =>
                !identity.LoadContext.TryGetTarget(out _));
            if (identities.Count == 0)
            {
                ActiveLoadContextIdentities.Remove(name);
            }
        }
    }

    private sealed record ActiveLoadContextIdentity(
        long Sequence,
        WeakReference<AssemblyLoadContext> LoadContext);

    private sealed record LoadContextIdentity(string Name, long Sequence)
    {
        public string Value => $"{Name}#{Sequence}";
    }
}
