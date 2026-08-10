using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

/// <summary>
/// Provides secret discovery from IOptions objects and programmatic secret registration.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. The <see cref="AddSecret"/> and
/// <see cref="AddSecrets(IEnumerable{string})"/> methods can be called concurrently from multiple threads.
/// </para>
/// <para>
/// <b>Secret Sources:</b>
/// </para>
/// <list type="bullet">
/// <item>Properties marked with <see cref="SecretValueAttribute"/> on IOptions classes (discovered at initialization)</item>
/// <item>Leaf values beneath configured <see cref="SecretMaskingOptions.MaskedConfigurationSections"/> paths</item>
/// <item>Secrets registered programmatically via <see cref="ISecretRegistry"/> (can be added at any time)</item>
/// </list>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class SecretProvider : ISecretProvider, ISecretEmissionGuard, ISecretRegistry, IInitializer
{
    private static readonly AsyncLocal<DeferredRegistrationScope?> CurrentDeferredRegistrationScope = new();
    private static readonly AsyncLocal<UnscopedRegistrationContext?> CurrentUnscopedRegistrationContext = new();

    private static readonly ConditionalWeakTable<Assembly, SecretAttributeReference> SecretAttributeReferenceCache = [];
    private static readonly ConditionalWeakTable<Type, ReflectionAccessors> ReflectionAccessorsCache = [];

    private readonly IOptionsProvider _optionsProvider;
    private readonly IBuildSystemSecretMasker _buildSystemSecretMasker;
    private readonly IOptions<SecretMaskingOptions> _maskingOptions;
    private readonly ILogger<SecretProvider> _logger;
    private readonly IConfiguration? _configuration;
    private readonly HashSet<string> _secrets = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, byte> _nativeMaskPatterns = new();
    private readonly ConcurrentDictionary<string, byte> _shortSecretWarnings = new();
    private readonly object _initLock = new();
    private readonly object _secretsLock = new();
    private readonly object _emissionStateLock = new();
    private readonly ReaderWriterLockSlim _secretEmissionLock = new();
    private readonly List<IReadOnlyList<string>> _deferredPatternsPendingPublication = [];

    private long _version;
    private long _nextDeferredSnapshotVersion;
    private long _deferredRegistrationBatch;
    private int _activeEmissionCount;
    private bool _deferredPublicationInProgress;
    // Direct redactors and a registering worker's next emission must see deferred
    // patterns immediately. Existing emissions keep the snapshot captured by their scope.
    private SecretSnapshot? _deferredMaskingSnapshot;
    private volatile bool _initialized;

    /// <inheritdoc />
    public long Version => FindDeferredRegistrationScope()?.Snapshot.Version
                           ?? GetMaskingSnapshotVersion();

    /// <summary>
    /// Gets all registered secrets.
    /// </summary>
    /// <remarks>
    /// <b>Thread Safety:</b> Enumeration returns a point-in-time snapshot of secrets.
    /// Secrets added during enumeration will not be included in the current iteration.
    /// Each enumeration creates a new snapshot.
    /// </remarks>
    public IEnumerable<string> Secrets => GetPublishedSnapshot().Secrets;

    public SecretProvider(
        IOptionsProvider optionsProvider,
        IBuildSystemSecretMasker buildSystemSecretMasker,
        IOptions<SecretMaskingOptions> maskingOptions,
        ILogger<SecretProvider> logger,
        IConfiguration? configuration = null)
    {
        _optionsProvider = optionsProvider;
        _buildSystemSecretMasker = buildSystemSecretMasker;
        _maskingOptions = maskingOptions;
        _logger = logger;
        _configuration = configuration;
    }

    /// <inheritdoc />
    public void AddSecret(string? secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
        {
            return;
        }

        var patterns = SecretMaskingPatternGenerator.Generate(secret);
        RegisterNativeMaskPatterns(patterns);

        var minimumLength = Math.Max(1, _maskingOptions.Value.MinimumSecretLength);
        if (secret.Length < minimumLength)
        {
            if (_shortSecretWarnings.TryAdd(secret, 0))
            {
                _logger.LogWarning(
                    "A secret with length {SecretLength} is shorter than MinimumSecretLength {MinimumSecretLength}. " +
                    "Framework log masking is disabled for this value; native build-system masking was requested.",
                    secret.Length,
                    minimumLength);
            }

            return;
        }

        if (TryDeferRegistration(patterns))
        {
            return;
        }

        if (TryDeferUnscopedRegistration(patterns))
        {
            return;
        }

        PublishPatterns(patterns);
    }

    private void PublishPatterns(IReadOnlyList<string> patterns)
    {
        _secretEmissionLock.EnterWriteLock();
        try
        {
            PublishPatternsWithoutEmissionLock(patterns);
        }
        finally
        {
            _secretEmissionLock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    public bool TryExecuteIfVersionCurrent(long expectedVersion, Action action)
    {
        lock (_secretsLock)
        {
            if (Version != expectedVersion)
            {
                return false;
            }

            // Keep the action inside this lock: callers use it for small synchronous
            // publication steps that must remain atomic with the version check.
            action();
            return true;
        }
    }

    /// <inheritdoc />
    public void AddSecrets(IEnumerable<string?> secrets)
    {
        foreach (var secret in secrets)
        {
            AddSecret(secret);
        }
    }

    /// <inheritdoc />
    public void AddSecrets(params string?[] secrets)
    {
        AddSecrets((IEnumerable<string?>) secrets);
    }

    /// <inheritdoc />
    public SecretSnapshot GetSnapshot()
    {
        return FindDeferredRegistrationScope()?.Snapshot ?? GetMaskingSnapshot();
    }

    private SecretSnapshot GetPublishedSnapshot()
    {
        lock (_secretsLock)
        {
            return new SecretSnapshot(Volatile.Read(ref _version), _secrets.ToArray());
        }
    }

    public void ExecuteWithStableSecrets<TState>(TState state, Action<TState> processOutput)
    {
        ArgumentNullException.ThrowIfNull(processOutput);

        var existingScope = FindDeferredRegistrationScope();
        var ownsReadLock = existingScope is null;
        EnterStableEmission(ownsReadLock, GetUnscopedRegistrationBatch());
        var previousScope = CurrentDeferredRegistrationScope.Value;
        var scope = new DeferredRegistrationScope(
            this,
            previousScope,
            existingScope is null
                ? GetMaskingSnapshot()
                : GetNestedMaskingSnapshot(existingScope));
        CurrentDeferredRegistrationScope.Value = scope;
        try
        {
            processOutput(state);
        }
        finally
        {
            CurrentDeferredRegistrationScope.Value = previousScope;
            ExitStableEmission(ownsReadLock, scope);
        }
    }

    private SecretSnapshot GetNestedMaskingSnapshot(DeferredRegistrationScope existingScope)
    {
        // Scope draining moves patterns into the deferred snapshot under this lock.
        // Hold it across both reads so every pattern is observed in one source.
        lock (_emissionStateLock)
        {
            var existingSnapshot = existingScope.Snapshot;
            var secrets = existingSnapshot.Secrets.ToHashSet(StringComparer.Ordinal);
            var existingCount = secrets.Count;
            secrets.UnionWith((_deferredMaskingSnapshot ?? GetPublishedSnapshot()).Secrets);
            for (var scope = existingScope; scope is not null; scope = scope.Parent)
            {
                if (ReferenceEquals(scope.Provider, this))
                {
                    scope.AddPatternsTo(secrets);
                }
            }

            return secrets.Count == existingCount
                ? existingSnapshot
                : new SecretSnapshot(
                    GetNextDeferredSnapshotVersion(),
                    secrets.ToArray());
        }
    }

    private void EnterStableEmission(
        bool ownsReadLock,
        long? unscopedRegistrationBatch)
    {
        if (!ownsReadLock)
        {
            lock (_emissionStateLock)
            {
                _activeEmissionCount++;
            }

            return;
        }

        while (true)
        {
            lock (_emissionStateLock)
            {
                while (ShouldWaitForDeferredPublication(unscopedRegistrationBatch))
                {
                    Monitor.Wait(_emissionStateLock);
                }
            }

            _secretEmissionLock.EnterReadLock();
            bool shouldRetry;
            lock (_emissionStateLock)
            {
                shouldRetry = ShouldWaitForDeferredPublication(unscopedRegistrationBatch);
                if (!shouldRetry)
                {
                    _activeEmissionCount++;
                }
            }

            if (!shouldRetry)
            {
                return;
            }

            _secretEmissionLock.ExitReadLock();
        }
    }

    private bool ShouldWaitForDeferredPublication(long? unscopedRegistrationBatch)
    {
        return unscopedRegistrationBatch != _deferredRegistrationBatch
               && (_deferredPublicationInProgress
                   || _deferredPatternsPendingPublication.Count != 0);
    }

    private void ExitStableEmission(
        bool ownsReadLock,
        DeferredRegistrationScope scope)
    {
        IReadOnlyList<string>[]? patternsToPublish;
        lock (_emissionStateLock)
        {
            var unclaimedPatterns = scope.CloseAndTakePatterns()
                .Where(patterns => !TryDeferRegistration(patterns))
                .ToArray();
            if (unclaimedPatterns.Length > 0
                && _deferredPatternsPendingPublication.Count == 0)
            {
                _deferredRegistrationBatch++;
            }

            _deferredPatternsPendingPublication.AddRange(unclaimedPatterns);
            if (unclaimedPatterns.Length > 0)
            {
                UpdateDeferredMaskingSnapshot(unclaimedPatterns);
            }

            _activeEmissionCount--;
            patternsToPublish = TryBeginDeferredPublicationUnderLock();
        }

        if (ownsReadLock)
        {
            _secretEmissionLock.ExitReadLock();
        }

        if (patternsToPublish is not null)
        {
            PublishDeferredPatterns(patternsToPublish);
        }
    }

    private IReadOnlyList<string>[]? TryBeginDeferredPublicationUnderLock()
    {
        if (_activeEmissionCount != 0
            || _deferredPublicationInProgress
            || _deferredPatternsPendingPublication.Count == 0)
        {
            return null;
        }

        _deferredPublicationInProgress = true;
        var patternsToPublish = _deferredPatternsPendingPublication.ToArray();
        _deferredPatternsPendingPublication.Clear();
        return patternsToPublish;
    }

    private void PublishDeferredPatterns(IReadOnlyList<string>[] patternsToPublish)
    {
        while (true)
        {
            PublishPatterns(patternsToPublish);

            lock (_emissionStateLock)
            {
                if (_activeEmissionCount == 0
                    && _deferredPatternsPendingPublication.Count != 0)
                {
                    patternsToPublish = _deferredPatternsPendingPublication.ToArray();
                    _deferredPatternsPendingPublication.Clear();
                    continue;
                }

                _deferredPublicationInProgress = false;
                if (_deferredPatternsPendingPublication.Count == 0)
                {
                    _deferredMaskingSnapshot = null;
                }

                Monitor.PulseAll(_emissionStateLock);
                return;
            }
        }
    }

    private bool TryDeferUnscopedRegistration(IReadOnlyList<string> patterns)
    {
        lock (_emissionStateLock)
        {
            if (_activeEmissionCount == 0)
            {
                return false;
            }

            if (_deferredPatternsPendingPublication.Count == 0)
            {
                _deferredRegistrationBatch++;
            }

            _deferredPatternsPendingPublication.Add(patterns);
            UpdateDeferredMaskingSnapshot([patterns]);
            CurrentUnscopedRegistrationContext.Value = UpsertUnscopedRegistrationContext(
                CurrentUnscopedRegistrationContext.Value,
                _deferredRegistrationBatch);
            return true;
        }
    }

    private UnscopedRegistrationContext UpsertUnscopedRegistrationContext(
        UnscopedRegistrationContext? context,
        long batch)
    {
        if (context is null)
        {
            return new UnscopedRegistrationContext(this, batch, null);
        }

        if (ReferenceEquals(context.Provider, this))
        {
            return context.Batch == batch
                ? context
                : context with { Batch = batch };
        }

        var updatedParent = UpsertUnscopedRegistrationContext(context.Parent, batch);
        return ReferenceEquals(updatedParent, context.Parent)
            ? context
            : context with { Parent = updatedParent };
    }

    private long GetMaskingSnapshotVersion()
    {
        lock (_emissionStateLock)
        {
            return _deferredMaskingSnapshot?.Version ?? Volatile.Read(ref _version);
        }
    }

    private SecretSnapshot GetMaskingSnapshot()
    {
        lock (_emissionStateLock)
        {
            return _deferredMaskingSnapshot ?? GetPublishedSnapshot();
        }
    }

    private void UpdateDeferredMaskingSnapshot(
        IEnumerable<IReadOnlyList<string>> deferredPatterns)
    {
        var currentSecrets = _deferredMaskingSnapshot?.Secrets ?? GetPublishedSnapshot().Secrets;
        var combinedSecrets = currentSecrets
            .Concat(deferredPatterns.SelectMany(static patterns => patterns))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        _deferredMaskingSnapshot = new SecretSnapshot(
            GetNextDeferredSnapshotVersion(),
            combinedSecrets);
    }

    private long GetNextDeferredSnapshotVersion() =>
        Interlocked.Add(ref _nextDeferredSnapshotVersion, -2);

    private long? GetUnscopedRegistrationBatch()
    {
        for (var context = CurrentUnscopedRegistrationContext.Value;
             context is not null;
             context = context.Parent)
        {
            if (ReferenceEquals(context.Provider, this))
            {
                return context.Batch;
            }
        }

        return null;
    }

    private void PublishPatternsWithoutEmissionLock(IReadOnlyList<string> patterns)
    {
        lock (_secretsLock)
        {
            if (patterns.All(_secrets.Contains))
            {
                return;
            }

            // Odd versions mark an in-progress publication so readers cannot reuse
            // a cache while the matching secret collection is being updated.
            Interlocked.Increment(ref _version);
            foreach (var pattern in patterns)
            {
                _secrets.Add(pattern);
            }

            Interlocked.Increment(ref _version);
        }
    }

    private void PublishPatterns(IEnumerable<IReadOnlyList<string>> deferredPatterns)
    {
        foreach (var patterns in deferredPatterns)
        {
            PublishPatterns(patterns);
        }
    }

    private bool TryDeferRegistration(IReadOnlyList<string> patterns)
    {
        for (var scope = CurrentDeferredRegistrationScope.Value; scope is not null; scope = scope.Parent)
        {
            if (ReferenceEquals(scope.Provider, this)
                && scope.TryAddPatterns(patterns, GetNextDeferredSnapshotVersion))
            {
                return true;
            }
        }

        return false;
    }

    private DeferredRegistrationScope? FindDeferredRegistrationScope()
    {
        for (var scope = CurrentDeferredRegistrationScope.Value; scope is not null; scope = scope.Parent)
        {
            if (ReferenceEquals(scope.Provider, this) && scope.IsOpen)
            {
                return scope;
            }
        }

        return null;
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Processed C# assemblies require exact generated metadata. Unprocessed assemblies use a reflection fallback and are not trim-safe.")]
    public IEnumerable<string> GetSecretsInObject(object? value)
    {
        if (value is null)
        {
            yield break;
        }

        var type = value.GetType();
        if (!GeneratedSecretMetadata.TryGetAccessors(type, out var secretProperties))
        {
            var reflectionFallbackIsUnsafe = GeneratedSecretMetadata.IsGeneratedMetadataRequired(type.Assembly)
                                             || !RuntimeFeature.IsDynamicCodeSupported;
            if (GeneratedSecretMetadata.IsAssemblyProcessed(type.Assembly))
            {
                if (reflectionFallbackIsUnsafe)
                {
                    throw new MissingSecretMetadataException(type);
                }

                secretProperties = GetReflectionAccessors(type);
            }
            else if (CanTypeHierarchyReferenceSecretValueAttribute(type))
            {
                if (reflectionFallbackIsUnsafe)
                {
                    throw new MissingSecretMetadataException(type);
                }

                secretProperties = GetReflectionAccessors(type);
            }
            else
            {
                yield break;
            }
        }

        foreach (var property in secretProperties)
        {
            foreach (var secret in GetSecretsFromProperty(property, value))
            {
                yield return secret;
            }
        }
    }

    /// <inheritdoc />
    public Task InitializeAsync()
    {
        // Use double-checked locking pattern for thread-safety
        // The volatile read of _initialized before the lock provides a fast-path
        // for subsequent calls after initialization is complete
        if (_initialized)
        {
            return Task.CompletedTask;
        }

        lock (_initLock)
        {
            // Re-check inside lock to prevent race condition
            if (_initialized)
            {
                return Task.CompletedTask;
            }

            AddSecrets(GetSecrets(_optionsProvider.GetOptions()));
            AddSecrets(GetConfiguredSectionSecrets());

            _initialized = true;
        }

        return Task.CompletedTask;
    }

    private static IEnumerable<string> GetSecretsFromProperty(
        SecretPropertyAccessor property,
        object value)
    {
        var propertyValue = property.Getter(value);
        var secretValueKeys = property.SecretValueKeys ?? Array.Empty<string>();

        if (secretValueKeys.Count == 0)
        {
            foreach (var secret in NormalizeSecrets(propertyValue))
            {
                if (!string.IsNullOrWhiteSpace(secret))
                {
                    yield return secret;
                }
            }

            yield break;
        }

        var keyValues = propertyValue switch
        {
            KeyValue keyValue => [keyValue],
            IEnumerable<KeyValue> values => values,
            _ => [],
        };

        foreach (var keyValue in keyValues)
        {
            if (secretValueKeys.Any(secretKey => IsMatchingSecretKey(keyValue.Key, secretKey)) &&
                !string.IsNullOrWhiteSpace(keyValue.Value))
            {
                yield return keyValue.Value;
            }
        }
    }

    private static bool CanReferenceSecretValueAttribute(Assembly assembly) =>
        CanReferenceSecretValueAttribute(assembly, RuntimeFeature.IsDynamicCodeSupported);

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Generated assemblies register themselves. Reference inspection only excludes third-party assemblies that cannot declare SecretValue properties.")]
    internal static bool CanReferenceSecretValueAttribute(
        Assembly assembly,
        bool isDynamicCodeSupported)
    {
        if (!isDynamicCodeSupported)
        {
            // Native AOT cannot inspect references or reflect over unprocessed types. Treat
            // their secret metadata as unknown so the caller fails loudly instead of assuming
            // that values are safe to log.
            return true;
        }

        return SecretAttributeReferenceCache.GetValue(assembly, static candidate =>
        {
            var attributeAssembly = typeof(SecretValueAttribute).Assembly;
            if (candidate == attributeAssembly)
            {
                return new SecretAttributeReference(true);
            }

            var attributeAssemblyName = attributeAssembly.GetName().Name;
            var referencesAttribute = candidate.GetReferencedAssemblies().Any(
                reference => string.Equals(
                    reference.Name,
                    attributeAssemblyName,
                    StringComparison.Ordinal));
            return new SecretAttributeReference(referencesAttribute);
        }).Value;
    }

    private static bool CanTypeHierarchyReferenceSecretValueAttribute(Type type)
    {
        for (var currentType = type; currentType is not null; currentType = currentType.BaseType)
        {
            if (CanReferenceSecretValueAttribute(currentType.Assembly))
            {
                return true;
            }
        }

        return false;
    }

    [RequiresUnreferencedCode("Assemblies without generated secret metadata require reflection and are not trim-safe.")]
    private static IReadOnlyList<SecretPropertyAccessor> GetSecretProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Concat(type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
            .Where(property => property.GetCustomAttribute<SecretValueAttribute>() is not null)
            .Select(property => new SecretPropertyAccessor(
                property.Name,
                property.GetValue,
                property.GetCustomAttribute<SecretValueAttribute>()!.Keys))
            .ToArray();
    }

    [RequiresUnreferencedCode("Partial and non-C# types require reflection metadata.")]
    private static IReadOnlyList<SecretPropertyAccessor> GetReflectionAccessors(Type type) =>
        ReflectionAccessorsCache.GetValue(
            type,
            static candidate => new ReflectionAccessors(GetSecretProperties(candidate))).Value;

    private static bool IsMatchingSecretKey(string key, string secretKey)
    {
        if (string.Equals(key, secretKey, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var keySegments = SplitKeySegments(key);
        var secretSegments = SplitKeySegments(secretKey);
        if (secretSegments.Count == 0 || secretSegments.Count > keySegments.Count)
        {
            return false;
        }

        for (var start = 0; start <= keySegments.Count - secretSegments.Count; start++)
        {
            var allSegmentsMatch = true;
            for (var offset = 0; offset < secretSegments.Count; offset++)
            {
                if (!string.Equals(
                        secretSegments[offset],
                        keySegments[start + offset],
                        StringComparison.OrdinalIgnoreCase))
                {
                    allSegmentsMatch = false;
                    break;
                }
            }

            if (allSegmentsMatch)
            {
                return true;
            }
        }

        return false;
    }

    private static IReadOnlyList<string> SplitKeySegments(string key)
    {
        var segments = new List<string>();
        var segmentStart = 0;
        for (var index = 0; index < key.Length; index++)
        {
            if (key[index] is '.' or '_' or '-')
            {
                AddSegment(key, segmentStart, index, segments);
                segmentStart = index + 1;
                continue;
            }

            if (index > segmentStart
                && char.IsUpper(key[index])
                && (char.IsLower(key[index - 1])
                    || (char.IsUpper(key[index - 1])
                        && index + 1 < key.Length
                        && char.IsLower(key[index + 1]))))
            {
                AddSegment(key, segmentStart, index, segments);
                segmentStart = index;
            }
        }

        AddSegment(key, segmentStart, key.Length, segments);
        return segments;
    }

    private static void AddSegment(
        string key,
        int start,
        int end,
        ICollection<string> segments)
    {
        if (end > start)
        {
            segments.Add(key[start..end]);
        }
    }

    private static IEnumerable<string?> NormalizeSecrets(object? value)
    {
        if (value is CliValuePair pair)
        {
            yield return pair.First;
            yield return pair.Second;
            yield break;
        }

        if (value is string || value is IEnumerable<char> || value is not IEnumerable enumerable)
        {
            yield return NormalizeSecret(value);
            yield break;
        }

        foreach (var item in enumerable)
        {
            if (item is CliValuePair itemPair)
            {
                yield return itemPair.First;
                yield return itemPair.Second;
            }
            else
            {
                yield return NormalizeSecret(item);
            }
        }
    }

    private static string? NormalizeSecret(object? value)
    {
        return value switch
        {
            null => null,
            string secret => secret,
            char[] characters => new string(characters),
            Memory<char> characters => characters.ToString(),
            ReadOnlyMemory<char> characters => characters.ToString(),
            IEnumerable<char> characters => new string(characters.ToArray()),
            CliOptionValue optionValue => optionValue.Value,
            KeyValue keyValue => keyValue.Value,
            _ => value.ToString(),
        };
    }

    private IEnumerable<string> GetSecrets(IEnumerable<object?> options)
    {
        foreach (var option in options)
        {
            foreach (var secret in GetSecretsInObject(option))
            {
                yield return secret;
            }
        }
    }

    private IEnumerable<string?> GetConfiguredSectionSecrets()
    {
        if (_configuration is null)
        {
            yield break;
        }

        foreach (var sectionPath in _maskingOptions.Value.MaskedConfigurationSections
                     .Where(static path => !string.IsNullOrWhiteSpace(path))
                     .Distinct(StringComparer.OrdinalIgnoreCase))
        {
            foreach (var value in GetLeafValues(_configuration.GetSection(sectionPath)))
            {
                yield return value;
            }
        }
    }

    private static IEnumerable<string?> GetLeafValues(IConfigurationSection section)
    {
        yield return section.Value;

        foreach (var child in section.GetChildren())
        {
            foreach (var value in GetLeafValues(child))
            {
                yield return value;
            }
        }
    }

    private void RegisterNativeMaskPatterns(IEnumerable<string> patterns)
    {
        var newPatterns = patterns.Where(pattern => _nativeMaskPatterns.TryAdd(pattern, 0)).ToArray();
        if (newPatterns.Length > 0)
        {
            _buildSystemSecretMasker.MaskSecrets(newPatterns);
        }
    }

    private sealed record SecretAttributeReference(bool Value);

    private sealed record ReflectionAccessors(IReadOnlyList<SecretPropertyAccessor> Value);

    private sealed record UnscopedRegistrationContext(
        SecretProvider Provider,
        long Batch,
        UnscopedRegistrationContext? Parent);

    private sealed class DeferredRegistrationScope
    {
        private readonly object _syncRoot = new();
        private readonly List<IReadOnlyList<string>> _patterns = [];
        private SecretSnapshot _snapshot;
        private bool _isOpen = true;

        public SecretProvider Provider { get; }

        public DeferredRegistrationScope? Parent { get; }

        public SecretSnapshot Snapshot
        {
            get
            {
                lock (_syncRoot)
                {
                    return _snapshot;
                }
            }
        }

        public DeferredRegistrationScope(
            SecretProvider provider,
            DeferredRegistrationScope? parent,
            SecretSnapshot snapshot)
        {
            Provider = provider;
            Parent = parent;
            _snapshot = snapshot;
        }

        public bool IsOpen
        {
            get
            {
                lock (_syncRoot)
                {
                    return _isOpen;
                }
            }
        }

        public bool TryAddPatterns(
            IReadOnlyList<string> patterns,
            Func<long> getNextSnapshotVersion)
        {
            lock (_syncRoot)
            {
                if (!_isOpen)
                {
                    return false;
                }

                _patterns.Add(patterns);
                var secrets = _snapshot.Secrets
                    .Concat(patterns)
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();
                if (secrets.Length != _snapshot.Secrets.Count)
                {
                    _snapshot = new SecretSnapshot(getNextSnapshotVersion(), secrets);
                }

                return true;
            }
        }

        public IReadOnlyList<IReadOnlyList<string>> CloseAndTakePatterns()
        {
            IReadOnlyList<IReadOnlyList<string>> patterns;
            lock (_syncRoot)
            {
                _isOpen = false;
                patterns = TakePatternsUnderLock();
            }

            return patterns;
        }

        public void AddPatternsTo(ISet<string> secrets)
        {
            lock (_syncRoot)
            {
                foreach (var patterns in _patterns)
                {
                    foreach (var pattern in patterns)
                    {
                        secrets.Add(pattern);
                    }
                }
            }
        }

        private IReadOnlyList<IReadOnlyList<string>> TakePatternsUnderLock()
        {
            var patterns = _patterns.ToArray();
            _patterns.Clear();
            return patterns;
        }
    }
}
