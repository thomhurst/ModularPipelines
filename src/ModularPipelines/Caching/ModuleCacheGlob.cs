namespace ModularPipelines.Caching;

internal sealed class ModuleCacheGlob(string pattern, bool ignoreCase)
{
    public bool IsMatch(string path)
    {
        // matches[length] records whether the processed pattern matches that path prefix.
        // Each token visits each prefix once: O(pattern.Length * path.Length) work,
        // O(path.Length) space, and no backtracking or scheduling-dependent deadline.
        var matches = path.Length < 256
            ? stackalloc bool[path.Length + 1]
            : new bool[path.Length + 1];
        matches.Clear();
        matches[0] = true;

        for (var index = 0; index < pattern.Length; index++)
        {
            var character = pattern[index];
            if (character == '*')
            {
                var recursive = index + 1 < pattern.Length && pattern[index + 1] == '*';
                if (recursive)
                {
                    index++;
                }

                if (recursive && index + 1 < pattern.Length && pattern[index + 1] == '/')
                {
                    index++;
                    MatchDirectories(path, matches);
                }
                else
                {
                    for (var length = 1; length <= path.Length; length++)
                    {
                        matches[length] |= matches[length - 1]
                                           && (recursive || path[length - 1] != '/');
                    }
                }

                continue;
            }

            if (character == '?')
            {
                for (var length = path.Length; length > 0; length--)
                {
                    matches[length] = matches[length - 1] && path[length - 1] != '/';
                }

                matches[0] = false;
                continue;
            }

            var literalStart = index;
            while (index + 1 < pattern.Length && pattern[index + 1] is not ('*' or '?'))
            {
                index++;
            }

            var literal = pattern.AsSpan(literalStart, index - literalStart + 1);
            for (var length = path.Length; length >= literal.Length; length--)
            {
                matches[length] = matches[length - literal.Length]
                                  && path.AsSpan(length - literal.Length, literal.Length).Equals(
                                      literal,
                                      ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            }

            matches[..Math.Min(literal.Length, matches.Length)].Clear();
        }

        return matches[path.Length];
    }

    private static void MatchDirectories(string path, Span<bool> matches)
    {
        // **/ may consume nothing, or any prefix ending in a slash. Remember the
        // old row as we update it so a directory match never consumes part of a filename.
        var previous = matches[0];
        var reachable = false;
        for (var length = 1; length <= path.Length; length++)
        {
            var current = matches[length];
            reachable |= previous;
            matches[length] = current || (reachable && path[length - 1] == '/');
            previous = current;
        }
    }
}
