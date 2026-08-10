using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

/// <summary>
/// Obfuscates sensitive information in strings by replacing secret values with a mask.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. The <see cref="Obfuscate"/> method can be
/// called concurrently from multiple threads without external synchronization.
/// </para>
/// <para>
/// <b>Performance:</b> Secrets are sorted by length (longest first) so longer secrets
/// win when multiple patterns match at the same position. A cached
/// <see cref="SearchValues{T}"/> finds matching positions in a single forward pass,
/// regardless of whether matching is case-sensitive.
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class SecretObfuscator : ISecretObfuscator, IInitializer
{
    private readonly ISecretProvider _secretProvider;
    private readonly IOptions<SecretMaskingOptions> _maskingOptions;
    private readonly object _secretCacheLock = new();
    private readonly ConditionalWeakTable<object, OptionsSecretCache> _optionsSecretCaches = [];

    private SecretCache? _secretCache;

    public int Order => int.MaxValue;

    public bool HasSecrets => GetRegistrationState().HasSecrets;

    internal bool CaseInsensitive => _maskingOptions.Value.CaseInsensitive;

    public SecretObfuscator(
        ISecretProvider secretProvider,
        IOptions<SecretMaskingOptions> maskingOptions)
    {
        _secretProvider = secretProvider;
        _maskingOptions = maskingOptions;
    }

    public Task InitializeAsync()
    {
        // Build system masking is handled by SecretProvider:
        // - Secrets from options are masked during SecretProvider.InitializeAsync()
        // - Secrets added via AddSecret() are masked immediately when added
        // This prevents duplicate masking calls to the build system
        return Task.CompletedTask;
    }

    public string Obfuscate(string? input, object? optionsObject)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        var options = _maskingOptions.Value;
        // Ensure mask value is never empty to avoid removing secrets without masking
        var maskValue = GetMaskValue(options);
        var caseInsensitive = options.CaseInsensitive;

        var secretCache = GetSecretCache(optionsObject, options, caseInsensitive);
        if (secretCache.SearchValues is null ||
            !input.AsSpan().ContainsAny(secretCache.SearchValues))
        {
            return input;
        }

        return ObfuscateMatches(
            input,
            secretCache.Secrets,
            secretCache.SearchValues,
            maskValue,
            caseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
    }

    internal string ObfuscatePreservingMasks(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        var options = _maskingOptions.Value;
        var maskValue = GetMaskValue(options);
        var caseInsensitive = options.CaseInsensitive;
        var secretCache = GetSecretCache(null, options, caseInsensitive);
        if (secretCache.SearchValues is null
            || !input.AsSpan().ContainsAny(secretCache.SearchValues))
        {
            return input;
        }

        return ObfuscateMatches(
            input,
            secretCache.Secrets,
            secretCache.SearchValues,
            maskValue,
            caseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal,
            preserveExistingMasks: true);
    }

    internal MappedObfuscatedOutput ObfuscatePreservingMasksWithSourceMap(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return new MappedObfuscatedOutput(string.Empty, [0]);
        }

        var options = _maskingOptions.Value;
        var maskValue = GetMaskValue(options);
        var caseInsensitive = options.CaseInsensitive;
        var secretCache = GetSecretCache(null, options, caseInsensitive);
        if (secretCache.SearchValues is null
            || !input.AsSpan().ContainsAny(secretCache.SearchValues))
        {
            return CreateUnchangedSourceMap(input);
        }

        return ObfuscateMatchesWithSourceMap(
            input,
            secretCache.Secrets,
            secretCache.SearchValues,
            maskValue,
            caseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
    }

    internal SecretRegistrationState GetRegistrationState()
    {
        var cache = GetRegisteredSecretCache(_maskingOptions.Value.CaseInsensitive);
        return new SecretRegistrationState(cache.Version, cache.SearchValues is not null);
    }

    internal bool CanSafelyPreserveMasks(IReadOnlyList<string> secrets)
    {
        var maskValue = GetMaskValue(_maskingOptions.Value);
        var comparison = CaseInsensitive
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        return secrets.All(secret =>
            string.IsNullOrEmpty(secret) || !maskValue.Contains(secret, comparison));
    }

    internal SecretCache GetSecretCache(
        object? optionsObject,
        SecretMaskingOptions options,
        bool caseInsensitive)
    {
        var registeredSecrets = GetRegisteredSecretCache(caseInsensitive);
        if (optionsObject is null)
        {
            return registeredSecrets;
        }

        var minimumLength = Math.Max(1, options.MinimumSecretLength);
        var extraSecrets = _secretProvider.GetSecretsInObject(optionsObject)
            .Where(secret => secret.Length >= minimumLength)
            .Distinct(StringComparer.Ordinal)
            .Order(StringComparer.Ordinal)
            .ToArray();
        var optionsSecretCache = _optionsSecretCaches.GetValue(
            optionsObject,
            static _ => new OptionsSecretCache());

        lock (optionsSecretCache.SyncRoot)
        {
            var secretsChanged = optionsSecretCache.MinimumSecretLength != minimumLength
                                 || !optionsSecretCache.ExtraSecrets.SequenceEqual(
                                     extraSecrets,
                                     StringComparer.Ordinal);
            if (!secretsChanged
                && optionsSecretCache.Cache is { } currentCache
                && currentCache.Version == registeredSecrets.Version
                && currentCache.CaseInsensitive == caseInsensitive)
            {
                return currentCache;
            }

            if (secretsChanged)
            {
                optionsSecretCache.MinimumSecretLength = minimumLength;
                optionsSecretCache.ExtraSecrets = extraSecrets;
                optionsSecretCache.Patterns = extraSecrets
                    .SelectMany(SecretMaskingPatternGenerator.Generate)
                    .ToArray();
            }

            var missingPatterns = optionsSecretCache.Patterns
                .Where(pattern => !registeredSecrets.ExactSecrets.Contains(pattern))
                .ToArray();
            var cache = missingPatterns.Length == 0
                ? registeredSecrets
                : CreateSecretCache(
                    registeredSecrets.Secrets.Concat(missingPatterns),
                    registeredSecrets.Version,
                    caseInsensitive);
            optionsSecretCache.Cache = cache;
            return cache;
        }
    }

    private SecretCache GetRegisteredSecretCache(bool caseInsensitive)
    {
        var version = _secretProvider.Version;
        var currentCache = Volatile.Read(ref _secretCache);
        if (currentCache is not null &&
            (version & 1) == 0 &&
            currentCache.Version == version &&
            currentCache.CaseInsensitive == caseInsensitive &&
            _secretProvider.Version == version)
        {
            return currentCache;
        }

        lock (_secretCacheLock)
        {
            var snapshot = _secretProvider.GetSnapshot();
            currentCache = _secretCache;
            if (currentCache is not null &&
                currentCache.Version == snapshot.Version &&
                currentCache.CaseInsensitive == caseInsensitive)
            {
                return currentCache;
            }

            var newCache = CreateSecretCache(snapshot.Secrets, snapshot.Version, caseInsensitive);
            Volatile.Write(ref _secretCache, newCache);
            return newCache;
        }
    }

    private static string GetMaskValue(SecretMaskingOptions options) =>
        string.IsNullOrWhiteSpace(options.MaskValue) ? "**********" : options.MaskValue;

    private static SecretCache CreateSecretCache(
        IEnumerable<string> secrets,
        long version,
        bool caseInsensitive)
    {
        var comparer = caseInsensitive ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        var nonBlankSecrets = secrets
            .Where(secret => !string.IsNullOrWhiteSpace(secret))
            .ToArray();
        var orderedSecrets = nonBlankSecrets
            .Distinct(comparer)
            .OrderByDescending(secret => secret.Length)
            .ToArray();
        var comparison = caseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        var searchValues = orderedSecrets.Length == 0
            ? null
            : SearchValues.Create(orderedSecrets, comparison);

        return new SecretCache(
            version,
            caseInsensitive,
            orderedSecrets,
            nonBlankSecrets.ToHashSet(comparer),
            searchValues);
    }

    private static string ObfuscateMatches(
        string input,
        IReadOnlyList<string> secrets,
        SearchValues<string> searchValues,
        string maskValue,
        StringComparison comparison,
        bool preserveExistingMasks = false)
    {
        var existingMaskRanges = preserveExistingMasks
            ? GetMaskRanges(input, maskValue)
            : [];
        var maskRangeIndex = 0;
        var result = new StringBuilder(input.Length);
        var inputOffset = 0;
        while (inputOffset < input.Length)
        {
            var remainingInput = input.AsSpan(inputOffset);
            var relativeMatchIndex = remainingInput.IndexOfAny(searchValues);
            if (relativeMatchIndex < 0)
            {
                break;
            }

            var matchIndex = inputOffset + relativeMatchIndex;
            result.Append(input, inputOffset, matchIndex - inputOffset);

            var matchingInput = input.AsSpan(matchIndex);
            string? matchedSecret = null;
            foreach (var secret in secrets)
            {
                if (matchingInput.StartsWith(secret, comparison))
                {
                    matchedSecret = secret;
                    break;
                }
            }

            if (matchedSecret is null)
            {
                throw new InvalidOperationException("SearchValues returned a position without a matching secret.");
            }

            if (IsContainedInExistingMask(
                    existingMaskRanges,
                    ref maskRangeIndex,
                    matchIndex,
                    matchedSecret.Length))
            {
                result.Append(input, matchIndex, matchedSecret.Length);
                inputOffset = matchIndex + matchedSecret.Length;
                continue;
            }

            result.Append(maskValue);
            inputOffset = matchIndex + matchedSecret.Length;
        }

        result.Append(input, inputOffset, input.Length - inputOffset);

        return result.ToString();
    }

    private static MappedObfuscatedOutput ObfuscateMatchesWithSourceMap(
        string input,
        IReadOnlyList<string> secrets,
        SearchValues<string> searchValues,
        string maskValue,
        StringComparison comparison)
    {
        var existingMaskRanges = GetMaskRanges(input, maskValue);
        var maskRangeIndex = 0;
        var sourceToOutputByteOffsets = new int[input.Length + 1];
        var result = new StringBuilder(input.Length);
        var inputOffset = 0;
        var outputByteCount = 0;
        while (inputOffset < input.Length)
        {
            var remainingInput = input.AsSpan(inputOffset);
            var relativeMatchIndex = remainingInput.IndexOfAny(searchValues);
            if (relativeMatchIndex < 0)
            {
                break;
            }

            var matchIndex = inputOffset + relativeMatchIndex;
            AppendUnchangedWithSourceMap(
                result,
                input,
                inputOffset,
                matchIndex,
                sourceToOutputByteOffsets,
                ref outputByteCount);

            var matchingInput = input.AsSpan(matchIndex);
            string? matchedSecret = null;
            foreach (var secret in secrets)
            {
                if (matchingInput.StartsWith(secret, comparison))
                {
                    matchedSecret = secret;
                    break;
                }
            }

            if (matchedSecret is null)
            {
                throw new InvalidOperationException("SearchValues returned a position without a matching secret.");
            }

            var matchEnd = matchIndex + matchedSecret.Length;
            if (IsContainedInExistingMask(
                    existingMaskRanges,
                    ref maskRangeIndex,
                    matchIndex,
                    matchedSecret.Length))
            {
                AppendUnchangedWithSourceMap(
                    result,
                    input,
                    matchIndex,
                    matchEnd,
                    sourceToOutputByteOffsets,
                    ref outputByteCount);
            }
            else
            {
                for (var index = matchIndex; index < matchEnd; index++)
                {
                    sourceToOutputByteOffsets[index] = outputByteCount;
                }

                result.Append(maskValue);
                outputByteCount += Encoding.UTF8.GetByteCount(maskValue);
                sourceToOutputByteOffsets[matchEnd] = outputByteCount;
            }

            inputOffset = matchEnd;
        }

        AppendUnchangedWithSourceMap(
            result,
            input,
            inputOffset,
            input.Length,
            sourceToOutputByteOffsets,
            ref outputByteCount);
        return new MappedObfuscatedOutput(result.ToString(), sourceToOutputByteOffsets);
    }

    private static MappedObfuscatedOutput CreateUnchangedSourceMap(string input)
    {
        var sourceToOutputByteOffsets = new int[input.Length + 1];
        var outputByteCount = 0;
        AppendUnchangedWithSourceMap(
            result: null,
            input,
            0,
            input.Length,
            sourceToOutputByteOffsets,
            ref outputByteCount);
        return new MappedObfuscatedOutput(input, sourceToOutputByteOffsets);
    }

    private static void AppendUnchangedWithSourceMap(
        StringBuilder? result,
        string input,
        int start,
        int end,
        int[] sourceToOutputByteOffsets,
        ref int outputByteCount)
    {
        result?.Append(input, start, end - start);
        for (var index = start; index < end;)
        {
            sourceToOutputByteOffsets[index] = outputByteCount;
            var status = Rune.DecodeFromUtf16(input.AsSpan(index, end - index), out var rune, out var consumed);
            if (status != OperationStatus.Done)
            {
                rune = Rune.ReplacementChar;
                consumed = 1;
            }

            for (var continuation = 1; continuation < consumed; continuation++)
            {
                sourceToOutputByteOffsets[index + continuation] = outputByteCount;
            }

            outputByteCount += rune.Utf8SequenceLength;
            index += consumed;
        }

        sourceToOutputByteOffsets[end] = outputByteCount;
    }

    private static bool IsContainedInExistingMask(
        IReadOnlyList<(int Start, int End)> ranges,
        ref int rangeIndex,
        int matchIndex,
        int matchLength)
    {
        while (rangeIndex < ranges.Count && ranges[rangeIndex].End <= matchIndex)
        {
            rangeIndex++;
        }

        var matchEnd = matchIndex + matchLength;
        for (var index = rangeIndex;
             index < ranges.Count && ranges[index].Start <= matchIndex;
             index++)
        {
            if (matchEnd <= ranges[index].End)
            {
                return true;
            }
        }

        return false;
    }

    private static IReadOnlyList<(int Start, int End)> GetMaskRanges(string input, string maskValue)
    {
        var ranges = new List<(int Start, int End)>();
        for (var index = input.IndexOf(maskValue, StringComparison.Ordinal);
             index >= 0;
             index = input.IndexOf(maskValue, index + 1, StringComparison.Ordinal))
        {
            ranges.Add((index, index + maskValue.Length));
        }

        return ranges;
    }

    private sealed class OptionsSecretCache
    {
        public object SyncRoot { get; } = new();

        public int MinimumSecretLength { get; set; }

        public string[] ExtraSecrets { get; set; } = [];

        public string[] Patterns { get; set; } = [];

        public SecretCache? Cache { get; set; }
    }

    internal sealed record SecretCache(
        long Version,
        bool CaseInsensitive,
        string[] Secrets,
        IReadOnlySet<string> ExactSecrets,
        SearchValues<string>? SearchValues);

    internal sealed record MappedObfuscatedOutput(
        string Value,
        IReadOnlyList<int> SourceToOutputByteOffsets)
    {
        public int Utf8ByteCount => SourceToOutputByteOffsets[^1];

        public int GetSuffixByteCount(int sourceOffset) =>
            Utf8ByteCount - SourceToOutputByteOffsets[sourceOffset];
    }

    internal readonly record struct SecretRegistrationState(long Version, bool HasSecrets);
}
