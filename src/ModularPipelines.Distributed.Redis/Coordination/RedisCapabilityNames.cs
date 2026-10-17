using System.Text.Json;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>Produces comparison keys for Lua without changing the capabilities' display names.</summary>
internal static class RedisCapabilityNames
{
    public static string[] Normalize(IEnumerable<Capability> capabilities) =>
        [.. capabilities.Select(static capability => Normalize(capability.Name))];

    private static string Normalize(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new JsonException("A capability name cannot be empty.");
        }

        return string.Create(name.Length, name, static (destination, source) =>
        {
            source.AsSpan().ToUpperInvariant(destination);
            for (var index = 0; index < source.Length;)
            {
                var length = char.IsHighSurrogate(source[index])
                    && index + 1 < source.Length && char.IsLowSurrogate(source[index + 1]) ? 2 : 1;
                var original = source.AsSpan(index, length);
                var upper = destination.Slice(index, length);
                // OrdinalIgnoreCase excludes some invariant case mappings, such as long s to ASCII S.
                if (!original.Equals(upper, StringComparison.OrdinalIgnoreCase))
                {
                    original.CopyTo(upper);
                }

                index += length;
            }
        });
    }
}
