using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using ModularPipelines.Secrets;

namespace ModularPipelines.Console;

/// <summary>
/// Intercepts Console.Out/Error writes and routes them through the coordinator.
/// </summary>
/// <remarks>
/// <para>
/// <b>Purpose:</b> This writer replaces Console.Out/Error to catch all direct
/// console writes. During progress phase, writes are buffered per-module.
/// After progress ends, writes pass through directly.
/// </para>
/// <para>
/// <b>Module Detection:</b> Uses the ambient module output context
/// to detect which module (if any) is currently executing. This allows Console.WriteLine
/// calls inside modules to be attributed to the correct module's output buffer.
/// </para>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. All operations are either
/// read-only or delegated to thread-safe components.
/// </para>
/// <para>
/// <b>Lock ordering:</b> Operations acquire the flush lock before a line-buffer lock.
/// Tracked obfuscation may then acquire the secret-emission guard and pattern-cache lock
/// before the output lock. Custom obfuscation uses its serialization lock outside both
/// the secret-emission guard and output lock. Locks must not be acquired in reverse order.
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
internal class CoordinatedTextWriter : TextWriter
{
    private static readonly AsyncLocal<ActiveOutputWriter?> ActiveOutputWriterScope = new();
    private static readonly AsyncLocal<bool> DirectWriteScope = new();

    private readonly IConsoleCoordinator _coordinator;
    private readonly TextWriter _realConsole;
    private readonly Func<bool> _shouldBuffer;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly ISecretProvider _secretProvider;
    private readonly bool _isError;
    private readonly Dictionary<LineBufferKey, LineBufferState> _lineBuffers = [];
    private readonly ConditionalWeakTable<object, Dictionary<LineBufferKey, LineBufferState>>
        _scopedLineBuffers = [];
    private readonly HashSet<LineBufferState> _pendingScopedLineBuffers = [];
    private readonly object _lineBufferLock = new();
    private readonly object _customObfuscatorLock = new();
    private readonly object _deferredOutputLock = new();
    private readonly object _secretPatternsLock = new();
    private readonly AsyncLocal<CustomObfuscationScope?> _customObfuscationScope = new();
    private readonly Queue<DeferredInputWrite> _deferredInputWrites = [];
    private readonly Queue<DeferredOutputWrite> _deferredOutputWrites = [];
    private readonly SemaphoreSlim _outputLock = new(1, 1);
    private readonly ReaderWriterLockSlim _flushLock = new(LockRecursionPolicy.SupportsRecursion);
    private volatile bool _isOutputSinkActive;
    private bool _isDeferredInputDrainActive;
    private SecretPatterns _secretPatterns = new([], null);
    private long _secretPatternsVersion = long.MinValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="CoordinatedTextWriter"/> class.
    /// Initializes a new coordinated text writer.
    /// </summary>
    /// <param name="coordinator">The console coordinator.</param>
    /// <param name="realConsole">The real console to write to when not buffering.</param>
    /// <param name="shouldBuffer">Function that returns whether output should be buffered.</param>
    /// <param name="secretObfuscator">Obfuscator for secrets in output.</param>
    /// <param name="secretProvider">Provider for registered secret patterns.</param>
    /// <param name="isError">Whether this writer represents standard error.</param>
    public CoordinatedTextWriter(
        IConsoleCoordinator coordinator,
        TextWriter realConsole,
        Func<bool> shouldBuffer,
        ISecretObfuscator secretObfuscator,
        ISecretProvider secretProvider,
        bool isError = false)
    {
        _coordinator = coordinator;
        _realConsole = realConsole;
        _shouldBuffer = shouldBuffer;
        _secretObfuscator = secretObfuscator;
        _secretProvider = secretProvider;
        _isError = isError;
    }

    /// <inheritdoc />
    public override Encoding Encoding => _realConsole.Encoding;

    /// <inheritdoc />
    public override void WriteLine(string? value)
    {
        if (TryDeferInputWrite((value ?? string.Empty).AsSpan(), appendNewLine: true))
        {
            return;
        }

        try
        {
            WriteSynchronized(
                GetLineBufferState(),
                (value ?? string.Empty).AsSpan(),
                appendNewLine: true);
        }
        finally
        {
            DrainDeferredInputWrites();
        }
    }

    /// <inheritdoc />
    public override void WriteLine()
    {
        WriteLine(string.Empty);
    }

    /// <inheritdoc />
    public override void Write(string? value)
    {
        if (value == null)
        {
            return;
        }

        if (TryDeferInputWrite(value.AsSpan(), appendNewLine: false))
        {
            return;
        }

        try
        {
            WriteSynchronized(GetLineBufferState(), value.AsSpan(), appendNewLine: false);
        }
        finally
        {
            DrainDeferredInputWrites();
        }
    }

    /// <inheritdoc />
    public override void Write(char value)
    {
        if (TryDeferInputWrite(stackalloc char[] { value }, appendNewLine: false))
        {
            return;
        }

        try
        {
            WriteSynchronized(
                GetLineBufferState(),
                stackalloc char[] { value },
                appendNewLine: false);
        }
        finally
        {
            DrainDeferredInputWrites();
        }
    }

    /// <inheritdoc />
    public override void Write(char[] buffer, int index, int count)
    {
        ArgumentNullException.ThrowIfNull(buffer);
        var value = buffer.AsSpan(index, count);
        if (TryDeferInputWrite(value, appendNewLine: false))
        {
            return;
        }

        try
        {
            WriteSynchronized(GetLineBufferState(), value, appendNewLine: false);
        }
        finally
        {
            DrainDeferredInputWrites();
        }
    }

    /// <inheritdoc />
    public override void Write(ReadOnlySpan<char> buffer)
    {
        if (TryDeferInputWrite(buffer, appendNewLine: false))
        {
            return;
        }

        try
        {
            WriteSynchronized(GetLineBufferState(), buffer, appendNewLine: false);
        }
        finally
        {
            DrainDeferredInputWrites();
        }
    }

    private void WriteSynchronized(
        LineBufferState state,
        ReadOnlySpan<char> value,
        bool appendNewLine)
    {
        _flushLock.EnterReadLock();
        try
        {
            lock (state.SyncRoot)
            {
                WriteCore(state, value, appendNewLine);
            }
        }
        finally
        {
            _flushLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Routes a message to the appropriate buffer based on current module context.
    /// </summary>
    private void RouteToBuffer(string message, Type? moduleType, bool appendNewLine)
    {
        var buffer = moduleType is not null
            ? _coordinator.GetModuleBuffer(moduleType)
            : _coordinator.GetUnattributedBuffer();
        if (buffer is IPreObfuscatedModuleOutputBuffer preObfuscatedBuffer)
        {
            preObfuscatedBuffer.WritePreObfuscated(
                message,
                _isError ? ModuleOutputStream.StandardError : ModuleOutputStream.StandardOutput,
                appendNewLine);
            return;
        }

        if (_isError)
        {
            if (appendNewLine)
            {
                buffer.WriteErrorLine(message);
            }
            else
            {
                buffer.WriteError(message);
            }
        }
        else if (appendNewLine)
        {
            buffer.WriteLine(message);
        }
        else
        {
            buffer.Write(message);
        }
    }

    private void WriteCore(LineBufferState state, ReadOnlySpan<char> value, bool appendNewLine)
    {
        var shouldBuffer = GetBufferMode(state, ShouldBuffer());
        var consumedLength = 0;
        while (consumedLength < value.Length)
        {
            var newlineIndex = value[consumedLength..].IndexOf('\n');
            if (newlineIndex < 0)
            {
                break;
            }

            var segmentLength = newlineIndex + 1;
            state.Buffer.Append(value.Slice(consumedLength, segmentLength));
            consumedLength += segmentLength;
            ProcessPendingOutput(state, shouldBuffer, shouldProcess: true);
        }

        state.Buffer.Append(value[consumedLength..]);

        if (appendNewLine)
        {
            state.Buffer.Append(Environment.NewLine);
            ProcessPendingOutput(state, shouldBuffer, shouldProcess: true);
        }
        else
        {
            ProcessPendingOutput(state, shouldBuffer, shouldProcess: false);
        }

        UpdateScopedLineBufferRetention(state);
    }

    private void ProcessPendingOutput(LineBufferState state, bool shouldBuffer, bool shouldProcess)
    {
        if (shouldBuffer && !shouldProcess)
        {
            return;
        }

        ExecuteWithStableSecrets(
            (Writer: this, State: state, ShouldBuffer: shouldBuffer),
            static context =>
            {
                var patterns = context.Writer.ObfuscateWithCurrentPatterns(
                    context.State,
                    preservePotentialLongerMatch: true,
                    out var retainedPrefixLength);
                context.Writer.FlushSafeOutput(
                    context.State,
                    retainedPrefixLength,
                    context.ShouldBuffer,
                    patterns);
            });
    }

    private SecretPatterns ObfuscateWithCurrentPatterns(
        LineBufferState state,
        bool preservePotentialLongerMatch,
        out int retainedPrefixLength)
    {
        string? unmodifiedBuffer = null;
        while (true)
        {
            var patterns = GetSecretPatterns();
            retainedPrefixLength = GetPotentialPatternPrefixLength(state.Buffer, patterns.Values);
            retainedPrefixLength = ObfuscateCompletePatterns(
                state,
                patterns,
                retainedPrefixLength,
                preservePotentialLongerMatch,
                out var bufferBeforeObfuscation);
            if (_secretProvider.Version == patterns.Version
                && GetPatternComparison() == patterns.Comparison)
            {
                return patterns;
            }

            unmodifiedBuffer ??= bufferBeforeObfuscation;
            if (unmodifiedBuffer is not null)
            {
                state.Buffer.Clear();
                state.Buffer.Append(unmodifiedBuffer);
            }
        }
    }

    private LineBufferState GetLineBufferState()
    {
        var moduleType = AmbientModuleOutputContext.Current?.ModuleType;
        var customObfuscationScope = _customObfuscationScope.Value;
        var outputWriteScope = GetOutputWriteBufferScope();
        var primaryScope = (object?) outputWriteScope ?? customObfuscationScope;
        var key = new LineBufferKey(
            moduleType,
            DirectWriteScope.Value,
            outputWriteScope is null ? null : customObfuscationScope);

        lock (_lineBufferLock)
        {
            var lineBuffers = primaryScope is null
                ? _lineBuffers
                : _scopedLineBuffers.GetValue(primaryScope, static _ => []);
            if (!lineBuffers.TryGetValue(key, out var state))
            {
                state = new LineBufferState(moduleType, primaryScope is not null);
                lineBuffers.Add(key, state);
            }

            return state;
        }
    }

    private static bool GetBufferMode(LineBufferState state, bool requestedBufferMode)
    {
        if (state.ShouldBuffer is null || state.Buffer.Length == 0)
        {
            state.ShouldBuffer = requestedBufferMode;
        }

        return state.ShouldBuffer.Value;
    }

    private SecretPatterns GetSecretPatterns()
    {
        var version = _secretProvider.Version;
        var comparison = GetPatternComparison();
        if ((version & 1) == 0
            && Volatile.Read(ref _secretPatternsVersion) == version
            && Volatile.Read(ref _secretPatterns).Comparison == comparison
            && _secretProvider.Version == version)
        {
            return Volatile.Read(ref _secretPatterns);
        }

        lock (_secretPatternsLock)
        {
            var snapshot = _secretProvider.GetSnapshot();
            comparison = GetPatternComparison();
            if (_secretPatternsVersion == snapshot.Version
                && _secretPatterns.Comparison == comparison)
            {
                return _secretPatterns;
            }

            var comparer = comparison == StringComparison.Ordinal
                ? StringComparer.Ordinal
                : StringComparer.OrdinalIgnoreCase;
            var values = (snapshot.Secrets ?? [])
                .Where(pattern => !string.IsNullOrEmpty(pattern))
                .Distinct(comparer)
                .OrderByDescending(pattern => pattern.Length)
                .ToArray();
            var patterns = new SecretPatterns(
                values,
                values.Length == 0
                    ? null
                    : SearchValues.Create(values, comparison),
                comparison,
                snapshot.Version);
            Volatile.Write(ref _secretPatterns, patterns);
            Volatile.Write(ref _secretPatternsVersion, snapshot.Version);
            return patterns;
        }
    }

    private int ObfuscateCompletePatterns(
        LineBufferState state,
        SecretPatterns patterns,
        int retainedPrefixLength,
        bool preservePotentialLongerMatch,
        out string? bufferBeforeObfuscation)
    {
        bufferBeforeObfuscation = null;
        if (state.Buffer.Length == 0 || patterns.SearchValues is null)
        {
            return retainedPrefixLength;
        }

        if (_secretObfuscator is not ITrackedSecretObfuscator trackedObfuscator)
        {
            return retainedPrefixLength;
        }

        var pending = GetPendingWithPatternCandidate(state.Buffer, patterns.SearchValues);
        if (pending is null)
        {
            return retainedPrefixLength;
        }

        var output = new StringBuilder(pending.Length);
        var outputIndex = 0;
        var searchIndex = 0;
        var replaced = false;
        var retainedPrefixInvalidated = false;
        var retainedPrefixStart = pending.Length - retainedPrefixLength;

        while (searchIndex < pending.Length)
        {
            var match = FindFirstPattern(pending, patterns, searchIndex);
            if (match.Index < 0)
            {
                break;
            }

            if (!TrySelectMaskableMatch(
                    pending,
                    patterns,
                    preservePotentialLongerMatch,
                    retainedPrefixInvalidated,
                    retainedPrefixLength,
                    retainedPrefixStart,
                    ref match))
            {
                searchIndex = match.Index + 1;
                continue;
            }

            var secret = pending.Substring(match.Index, match.Length);
            var obfuscation = trackedObfuscator.ObfuscateWithConsumption(secret, null);
            if (obfuscation.ConsumedInputLength == 0)
            {
                searchIndex = match.Index + 1;
                continue;
            }

            var unconsumedLength = match.Length - obfuscation.ConsumedInputLength;
            var obfuscatedLength = obfuscation.Output.Length - unconsumedLength;
            output.Append(pending, outputIndex, match.Index - outputIndex);
            output.Append(obfuscation.Output, 0, obfuscatedLength);
            retainedPrefixInvalidated |= match.Index + obfuscation.ConsumedInputLength > retainedPrefixStart;
            outputIndex = match.Index + obfuscation.ConsumedInputLength;
            searchIndex = outputIndex;
            replaced = true;
        }

        if (replaced)
        {
            bufferBeforeObfuscation = pending;
            output.Append(pending, outputIndex, pending.Length - outputIndex);
            state.Buffer.Clear();
            state.Buffer.Append(output);
        }

        return retainedPrefixInvalidated
            ? GetPotentialPatternPrefixLength(state.Buffer, patterns.Values)
            : retainedPrefixLength;
    }

    private static bool TrySelectMaskableMatch(
        string pending,
        SecretPatterns patterns,
        bool preservePotentialLongerMatch,
        bool retainedPrefixInvalidated,
        int retainedPrefixLength,
        int retainedPrefixStart,
        ref (int Index, int Length) match)
    {
        if (!preservePotentialLongerMatch
            || retainedPrefixInvalidated
            || retainedPrefixLength == 0
            || match.Index + match.Length <= retainedPrefixStart)
        {
            return true;
        }

        var safeMatchLength = FindLongestPatternEndingAtOrBefore(
            pending,
            patterns.Values,
            match.Index,
            retainedPrefixStart,
            patterns.Comparison);
        if (safeMatchLength > 0)
        {
            match = (match.Index, safeMatchLength);
            return true;
        }

        return match.Index < retainedPrefixStart
               && !HasCompletePatternBetween(
                   pending,
                   patterns,
                   match.Index + 1,
                   retainedPrefixStart);
    }

    private static string? GetPendingWithPatternCandidate(
        StringBuilder buffer,
        SearchValues<string> searchValues)
    {
        var chunks = buffer.GetChunks();
        if (!chunks.MoveNext())
        {
            return null;
        }

        var firstChunk = chunks.Current.Span;
        if (!chunks.MoveNext())
        {
            return firstChunk.IndexOfAny(searchValues) >= 0
                ? buffer.ToString()
                : null;
        }

        var pending = buffer.ToString();
        return pending.AsSpan().IndexOfAny(searchValues) >= 0
            ? pending
            : null;
    }

    private void FlushSafeOutput(
        LineBufferState state,
        int retainedLength,
        bool shouldBuffer,
        SecretPatterns secretPatterns)
    {
        FlushSafePrefix(
            state,
            state.Buffer.Length - retainedLength,
            shouldBuffer,
            secretPatterns);

        if (state.Buffer.Length == 0)
        {
            state.ShouldBuffer = null;
        }
    }

    private void FlushSafePrefix(
        LineBufferState state,
        int safeLength,
        bool shouldBuffer,
        SecretPatterns secretPatterns)
    {
        var consumedLength = 0;
        for (var index = 0; index < safeLength; index++)
        {
            if (state.Buffer[index] != '\n')
            {
                continue;
            }

            var lineLength = index - consumedLength;
            while (lineLength > 0 && state.Buffer[consumedLength + lineLength - 1] == '\r')
            {
                lineLength--;
            }

            var line = state.Buffer.ToString(consumedLength, lineLength);
            WriteCompletedLine(line, shouldBuffer, state.ModuleType, secretPatterns);
            consumedLength = index + 1;
        }

        if (consumedLength > 0)
        {
            state.Buffer.Remove(0, consumedLength);
            safeLength -= consumedLength;
        }

        if (!shouldBuffer)
        {
            FlushDirectPrefix(state, safeLength, secretPatterns);
        }
    }

    private static (int Index, int Length) FindFirstPattern(
        string input,
        SecretPatterns patterns,
        int startIndex)
    {
        var relativeIndex = input.AsSpan(startIndex).IndexOfAny(patterns.SearchValues!);
        if (relativeIndex < 0)
        {
            return (-1, 0);
        }

        var firstIndex = startIndex + relativeIndex;
        var matchingInput = input.AsSpan(firstIndex);
        foreach (var pattern in patterns.Values)
        {
            if (matchingInput.StartsWith(pattern, patterns.Comparison))
            {
                return (firstIndex, pattern.Length);
            }
        }

        throw new InvalidOperationException("SearchValues returned a position without a matching secret.");
    }

    private static int FindLongestPatternEndingAtOrBefore(
        string input,
        IReadOnlyList<string> patterns,
        int startIndex,
        int endIndex,
        StringComparison comparison)
    {
        if (startIndex >= endIndex)
        {
            return 0;
        }

        var safeInput = input.AsSpan(startIndex, endIndex - startIndex);
        foreach (var pattern in patterns)
        {
            if (safeInput.StartsWith(pattern, comparison))
            {
                return pattern.Length;
            }
        }

        return 0;
    }

    private static bool HasCompletePatternBetween(
        string input,
        SecretPatterns patterns,
        int startIndex,
        int endIndex)
    {
        while (startIndex < endIndex)
        {
            var relativeIndex = input.AsSpan(startIndex, endIndex - startIndex)
                .IndexOfAny(patterns.SearchValues!);
            if (relativeIndex < 0)
            {
                return false;
            }

            var index = startIndex + relativeIndex;
            var safeInput = input.AsSpan(index, endIndex - index);
            foreach (var pattern in patterns.Values)
            {
                if (safeInput.StartsWith(pattern, patterns.Comparison))
                {
                    return true;
                }
            }

            startIndex = index + 1;
        }

        return false;
    }

    private int GetPotentialPatternPrefixLength(StringBuilder input, IReadOnlyList<string> patterns)
    {
        if (input.Length == 0 || patterns.Count == 0)
        {
            return 0;
        }

        var maximumLength = patterns[0].Length - 1;
        var suffixLength = Math.Min(input.Length, maximumLength);

        if (suffixLength <= 256)
        {
            Span<char> suffixBuffer = stackalloc char[suffixLength];
            input.CopyTo(input.Length - suffixLength, suffixBuffer, suffixLength);
            return FindPotentialPatternPrefixLength(suffixBuffer, patterns, GetPatternComparison());
        }

        var rentedBuffer = ArrayPool<char>.Shared.Rent(suffixLength);
        try
        {
            var suffixBuffer = rentedBuffer.AsSpan(0, suffixLength);
            input.CopyTo(input.Length - suffixLength, suffixBuffer, suffixLength);
            return FindPotentialPatternPrefixLength(suffixBuffer, patterns, GetPatternComparison());
        }
        finally
        {
            ArrayPool<char>.Shared.Return(rentedBuffer, clearArray: true);
        }
    }

    private static int FindPotentialPatternPrefixLength(
        ReadOnlySpan<char> suffixBuffer,
        IReadOnlyList<string> patterns,
        StringComparison comparison)
    {
        for (var length = suffixBuffer.Length; length > 0; length--)
        {
            var suffix = suffixBuffer[^length..];
            foreach (var pattern in patterns)
            {
                if (pattern.Length > length
                    && pattern.AsSpan().StartsWith(suffix, comparison))
                {
                    return length;
                }
            }
        }

        return 0;
    }

    private StringComparison GetPatternComparison() =>
        _secretObfuscator is ITrackedSecretObfuscator trackedObfuscator
            ? trackedObfuscator.PatternComparison
            : StringComparison.OrdinalIgnoreCase;

    private void FlushDirectPrefix(
        LineBufferState state,
        int length,
        SecretPatterns secretPatterns)
    {
        if (length <= 0)
        {
            return;
        }

        var output = state.Buffer.ToString(0, length);
        state.Buffer.Remove(0, length);
        WriteToRealConsole(
            ObfuscateCustomOutput(output, secretPatterns),
            appendNewLine: false);
    }

    private void WriteCompletedLine(
        string line,
        bool shouldBuffer,
        Type? moduleType,
        SecretPatterns secretPatterns)
    {
        if (shouldBuffer)
        {
            RouteToBuffer(
                ObfuscateCustomOutput(line, secretPatterns),
                moduleType,
                appendNewLine: true);
        }
        else
        {
            WriteToRealConsole(
                ObfuscateCustomOutput(line, secretPatterns),
                appendNewLine: true);
        }
    }

    private void FlushPartialLine(
        LineBufferState state,
        bool shouldBuffer,
        SecretPatterns secretPatterns)
    {
        FlushPartialPrefix(state, state.Buffer.Length, shouldBuffer, secretPatterns);
    }

    private void FlushPartialPrefix(
        LineBufferState state,
        int length,
        bool shouldBuffer,
        SecretPatterns secretPatterns)
    {
        if (length <= 0)
        {
            return;
        }

        var pending = state.Buffer.ToString(0, length);
        state.Buffer.Remove(0, length);

        if (shouldBuffer)
        {
            RouteToBuffer(
                ObfuscateCustomOutput(pending, secretPatterns),
                state.ModuleType,
                appendNewLine: false);
        }
        else
        {
            WriteToRealConsole(
                ObfuscateCustomOutput(pending, secretPatterns),
                appendNewLine: false);
        }
    }

    private void WriteToRealConsole(string output, bool appendNewLine)
    {
        if (IsReentrantOutputWrite())
        {
            var reentrantOutputWriter = EnterActiveOutputWriter();
            try
            {
                WriteToRealConsoleCore(output, appendNewLine);
            }
            finally
            {
                ExitActiveOutputWriter(reentrantOutputWriter);
            }

            return;
        }

        if (TryDeferOutputWrite(output, appendNewLine))
        {
            return;
        }

        _outputLock.Wait();
        var activeOutputWriter = EnterActiveOutputWriter();
        BeginDeferringOutputWrites();
        try
        {
            WriteToRealConsoleCore(output, appendNewLine);
        }
        finally
        {
            CompleteOutputWrite(activeOutputWriter);
        }
    }

    private void WriteToRealConsoleCore(string output, bool appendNewLine)
    {
        if (appendNewLine)
        {
            _realConsole.WriteLine(output);
        }
        else
        {
            _realConsole.Write(output);
        }
    }

    private bool TryDeferOutputWrite(string output, bool appendNewLine)
    {
        lock (_deferredOutputLock)
        {
            if (!_isOutputSinkActive)
            {
                return false;
            }

            // A sink can suppress ExecutionContext flow before waiting for work that
            // writes through this writer. That callback cannot see the active writer
            // scope, so queue its processed output instead of waiting on our lease.
            _deferredOutputWrites.Enqueue(new DeferredOutputWrite(output, appendNewLine));
            return true;
        }
    }

    private bool TryDeferInputWrite(ReadOnlySpan<char> value, bool appendNewLine)
    {
        if (IsReentrantOutputWrite() || !_isOutputSinkActive)
        {
            return false;
        }

        var state = GetLineBufferState();
        lock (_deferredOutputLock)
        {
            if (!_isOutputSinkActive)
            {
                return false;
            }

            _deferredInputWrites.Enqueue(
                new DeferredInputWrite(value.ToString(), appendNewLine, state));
            return true;
        }
    }

    private void DrainDeferredInputWrites()
    {
        lock (_deferredOutputLock)
        {
            if (_isDeferredInputDrainActive)
            {
                return;
            }

            _isDeferredInputDrainActive = true;
        }

        while (true)
        {
            DeferredInputWrite write;
            lock (_deferredOutputLock)
            {
                if (_deferredInputWrites.Count == 0)
                {
                    _isDeferredInputDrainActive = false;
                    return;
                }

                write = _deferredInputWrites.Dequeue();
            }

            try
            {
                WriteSynchronized(write.State, write.Output.AsSpan(), write.AppendNewLine);
            }
            catch
            {
                lock (_deferredOutputLock)
                {
                    _isDeferredInputDrainActive = false;
                }

                throw;
            }
        }
    }

    private void BeginDeferringOutputWrites()
    {
        lock (_deferredOutputLock)
        {
            _isOutputSinkActive = true;
        }
    }

    private void DrainDeferredOutputWrites()
    {
        while (true)
        {
            DeferredOutputWrite[] pending;
            lock (_deferredOutputLock)
            {
                if (_deferredOutputWrites.Count == 0)
                {
                    _isOutputSinkActive = false;
                    return;
                }

                pending = _deferredOutputWrites.ToArray();
                _deferredOutputWrites.Clear();
            }

            foreach (var write in pending)
            {
                WriteToRealConsoleCore(write.Output, write.AppendNewLine);
            }
        }
    }

    private void StopDeferringOutputWrites()
    {
        lock (_deferredOutputLock)
        {
            _isOutputSinkActive = false;
        }
    }

    private void CompleteOutputWrite(ActiveOutputWriter activeOutputWriter)
    {
        try
        {
            DrainDeferredOutputWrites();
        }
        finally
        {
            StopDeferringOutputWrites();
            ExitActiveOutputWriter(activeOutputWriter);
            _outputLock.Release();
        }
    }

    private bool IsReentrantOutputWrite() => GetReentrantOutputWriteDepth() > 0;

    private int GetReentrantOutputWriteDepth()
    {
        var depth = 0;
        for (var activeOutputWriter = ActiveOutputWriterScope.Value;
             activeOutputWriter != null;
             activeOutputWriter = activeOutputWriter.Parent)
        {
            if (activeOutputWriter.IsActive
                && ReferenceEquals(activeOutputWriter.Writer, this))
            {
                depth++;
            }
        }

        return depth;
    }

    private ActiveOutputWriter? GetOutputWriteBufferScope()
    {
        for (var activeOutputWriter = ActiveOutputWriterScope.Value;
             activeOutputWriter != null;
             activeOutputWriter = activeOutputWriter.Parent)
        {
            if (ReferenceEquals(activeOutputWriter.Writer, this))
            {
                return activeOutputWriter;
            }
        }

        return null;
    }

    private ActiveOutputWriter EnterActiveOutputWriter()
    {
        var activeOutputWriter = new ActiveOutputWriter(this, ActiveOutputWriterScope.Value);
        ActiveOutputWriterScope.Value = activeOutputWriter;
        return activeOutputWriter;
    }

    private static void ExitActiveOutputWriter(ActiveOutputWriter activeOutputWriter)
    {
        activeOutputWriter.Deactivate();
        ActiveOutputWriterScope.Value = activeOutputWriter.Parent;
    }

    private string ObfuscateCustomOutput(string output, SecretPatterns secretPatterns)
    {
        if (_secretObfuscator is ITrackedSecretObfuscator)
        {
            return _secretProvider.Version == secretPatterns.Version
                && GetPatternComparison() == secretPatterns.Comparison
                ? output
                : _secretObfuscator.Obfuscate(output, null);
        }

        lock (_customObfuscatorLock)
        {
            var scope = EnterCustomObfuscationScope();
            try
            {
                return _secretObfuscator.Obfuscate(output, null);
            }
            finally
            {
                ExitCustomObfuscationScope(scope);
            }
        }
    }

    private CustomObfuscationScope EnterCustomObfuscationScope()
    {
        var scope = new CustomObfuscationScope(_customObfuscationScope.Value);
        _customObfuscationScope.Value = scope;
        return scope;
    }

    private void ExitCustomObfuscationScope(CustomObfuscationScope scope)
    {
        scope.Deactivate();
        _customObfuscationScope.Value = scope.Parent;
    }

    private bool IsCustomObfuscationActive()
    {
        for (var scope = _customObfuscationScope.Value;
             scope != null;
             scope = scope.Parent)
        {
            if (scope.IsActive)
            {
                return true;
            }
        }

        return false;
    }

    private void ExecuteWithStableSecrets<TState>(TState state, Action<TState> processOutput)
    {
        if (_secretObfuscator is ITrackedSecretObfuscator
            && _secretProvider is ISecretEmissionGuard emissionGuard)
        {
            emissionGuard.ExecuteWithStableSecrets(state, processOutput);
            return;
        }

        processOutput(state);
    }

    /// <inheritdoc />
    public override void Flush()
    {
        try
        {
            if (IsReentrantOutputWrite())
            {
                FlushReentrantOutput();
                _realConsole.Flush();
                return;
            }

            if (IsCustomObfuscationActive())
            {
                FlushReentrantOutput();
                FlushRealConsole();
                return;
            }

            FlushBufferedOutput();
            FlushRealConsole();
        }
        finally
        {
            DrainDeferredInputWrites();
        }
    }

    private void FlushReentrantOutput()
    {
        // The outer write still owns a recursive read lock, so a full flush cannot
        // upgrade to the write lock. Drain only this reentrant context's state.
        var state = GetLineBufferState();
        lock (state.SyncRoot)
        {
            if (state.Buffer.Length > 0)
            {
                FlushState(state, retainPotentialPrefix: false);
            }
        }
    }

    private void FlushBufferedOutput()
    {
        _flushLock.EnterWriteLock();
        try
        {
            foreach (var state in GetLineBufferStates())
            {
                lock (state.SyncRoot)
                {
                    if (state.Buffer.Length == 0)
                    {
                        continue;
                    }

                    FlushState(state, retainPotentialPrefix: false);
                }
            }
        }
        finally
        {
            _flushLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Flushes output that cannot be a prefix of a registered secret while retaining
    /// incomplete secret prefixes for subsequent writes.
    /// </summary>
    internal async Task FlushAvailableAsync()
    {
        FlushAvailableOutput();
        await FlushRealConsoleAsync().ConfigureAwait(false);
    }

    private void FlushAvailableOutput()
    {
        _flushLock.EnterWriteLock();
        try
        {
            foreach (var state in GetLineBufferStates())
            {
                lock (state.SyncRoot)
                {
                    if (state.Buffer.Length == 0)
                    {
                        continue;
                    }

                    FlushState(state, retainPotentialPrefix: true);
                }
            }
        }
        finally
        {
            _flushLock.ExitWriteLock();
        }
    }

    private void FlushState(LineBufferState state, bool retainPotentialPrefix)
    {
        ExecuteWithStableSecrets(
            (Writer: this, State: state, RetainPotentialPrefix: retainPotentialPrefix),
            static context =>
            {
                var shouldBuffer = context.State.ShouldBuffer ?? context.Writer.ShouldBuffer();
                var patterns = context.Writer.ObfuscateWithCurrentPatterns(
                    context.State,
                    preservePotentialLongerMatch: false,
                    out var retainedLength);
                if (!context.RetainPotentialPrefix)
                {
                    context.Writer.FlushSafePrefix(
                        context.State,
                        context.State.Buffer.Length,
                        shouldBuffer,
                        patterns);
                    context.Writer.FlushPartialLine(
                        context.State,
                        shouldBuffer,
                        patterns);
                    context.State.ShouldBuffer = null;
                    return;
                }

                context.Writer.FlushSafeOutput(
                    context.State,
                    retainedLength,
                    shouldBuffer,
                    patterns);

                if (shouldBuffer)
                {
                    context.Writer.FlushPartialPrefix(
                        context.State,
                        context.State.Buffer.Length - retainedLength,
                        shouldBuffer,
                        patterns);
                }

                if (context.State.Buffer.Length == 0)
                {
                    context.State.ShouldBuffer = null;
                }
            });

        UpdateScopedLineBufferRetention(state);
    }

    /// <inheritdoc />
    public override async Task FlushAsync()
    {
        try
        {
            if (IsReentrantOutputWrite())
            {
                FlushReentrantOutput();
                await _realConsole.FlushAsync().ConfigureAwait(false);
                return;
            }

            if (IsCustomObfuscationActive())
            {
                FlushReentrantOutput();
                await FlushRealConsoleAsync().ConfigureAwait(false);
                return;
            }

            FlushBufferedOutput();
            await FlushRealConsoleAsync().ConfigureAwait(false);
        }
        finally
        {
            DrainDeferredInputWrites();
        }
    }

    private void FlushRealConsole()
    {
        _outputLock.Wait();
        var activeOutputWriter = EnterActiveOutputWriter();
        BeginDeferringOutputWrites();
        try
        {
            // Always flush real console (needed for Spectre.Console internals)
            _realConsole.Flush();
        }
        finally
        {
            CompleteOutputWrite(activeOutputWriter);
        }
    }

    private async Task FlushRealConsoleAsync()
    {
        await _outputLock.WaitAsync().ConfigureAwait(false);
        var activeOutputWriter = EnterActiveOutputWriter();
        BeginDeferringOutputWrites();
        try
        {
            await _realConsole.FlushAsync().ConfigureAwait(false);
        }
        finally
        {
            CompleteOutputWrite(activeOutputWriter);
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Flush();
        }

        base.Dispose(disposing);
    }

    internal static IDisposable BeginDirectWrite()
    {
        var previousValue = DirectWriteScope.Value;
        DirectWriteScope.Value = true;
        return new DirectWriteScopeRestorer(previousValue);
    }

    private bool ShouldBuffer() => !DirectWriteScope.Value && _shouldBuffer();

    private sealed class ActiveOutputWriter(
        CoordinatedTextWriter writer,
        ActiveOutputWriter? parent)
    {
        private int _isActive = 1;

        public CoordinatedTextWriter Writer { get; } = writer;

        public ActiveOutputWriter? Parent { get; } = parent;

        public bool IsActive => Volatile.Read(ref _isActive) != 0;

        public void Deactivate() => Volatile.Write(ref _isActive, 0);
    }

    private readonly record struct DeferredInputWrite(
        string Output,
        bool AppendNewLine,
        LineBufferState State);

    private readonly record struct DeferredOutputWrite(string Output, bool AppendNewLine);

    private LineBufferState[] GetLineBufferStates()
    {
        lock (_lineBufferLock)
        {
            return _lineBuffers.Values
                .Concat(_pendingScopedLineBuffers)
                .ToArray();
        }
    }

    private void UpdateScopedLineBufferRetention(LineBufferState state)
    {
        if (!state.IsScoped)
        {
            return;
        }

        lock (_lineBufferLock)
        {
            if (state.Buffer.Length == 0)
            {
                _pendingScopedLineBuffers.Remove(state);
            }
            else
            {
                _pendingScopedLineBuffers.Add(state);
            }
        }
    }

    private sealed class CustomObfuscationScope(CustomObfuscationScope? parent)
    {
        private int _isActive = 1;

        public CustomObfuscationScope? Parent { get; } = parent;

        public bool IsActive => Volatile.Read(ref _isActive) != 0;

        public void Deactivate() => Volatile.Write(ref _isActive, 0);
    }

    private sealed class LineBufferState(Type? moduleType, bool isScoped)
    {
        public object SyncRoot { get; } = new();

        public Type? ModuleType { get; } = moduleType;

        public bool IsScoped { get; } = isScoped;

        public StringBuilder Buffer { get; } = new();

        public bool? ShouldBuffer { get; set; }
    }

    private readonly record struct LineBufferKey(
        Type? ModuleType,
        bool IsDirectWrite,
        CustomObfuscationScope? NestedCustomObfuscationScope);

    private sealed record SecretPatterns(
        string[] Values,
        SearchValues<string>? SearchValues,
        StringComparison Comparison = StringComparison.OrdinalIgnoreCase,
        long Version = long.MinValue);

    private sealed class DirectWriteScopeRestorer(bool previousValue) : IDisposable
    {
        public void Dispose()
        {
            DirectWriteScope.Value = previousValue;
        }
    }
}
