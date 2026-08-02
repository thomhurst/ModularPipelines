using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using ModularPipelines.Constants;
using ModularPipelines.Engine;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides downstream loggers with an exception whose public text is safe to render.
/// </summary>
/// <remarks>
/// Wrapping intentionally replaces the original exception type identity.
/// </remarks>
internal sealed class ObfuscatedLogException : Exception
{
    private readonly string? _obfuscatedStackTrace;
    private readonly string _obfuscatedText;

    private ObfuscatedLogException(Exception exception, ISecretObfuscator secretObfuscator)
        : base(
            GetObfuscatedMessage(exception, secretObfuscator),
            Create(exception.InnerException, secretObfuscator))
    {
        _obfuscatedStackTrace = GetObfuscatedDiagnostic(
            () => exception.StackTrace,
            secretObfuscator);
        _obfuscatedText = GetObfuscatedText(exception, secretObfuscator);
        CopyDiagnostics(this, exception, secretObfuscator);
    }

    public static Exception? Create(Exception? exception, ISecretObfuscator secretObfuscator)
        => exception switch
        {
            null => null,
            AggregateException aggregateException =>
                new ObfuscatedAggregateLogException(aggregateException, secretObfuscator),
            _ => new ObfuscatedLogException(exception, secretObfuscator),
        };

    public override string? StackTrace => _obfuscatedStackTrace;

    public override string ToString() => _obfuscatedText;

    private static string? ObfuscateNullable(
        string? value,
        ISecretObfuscator secretObfuscator) =>
        value is null ? null : secretObfuscator.Obfuscate(value, null);

    private static string GetObfuscatedMessage(
        Exception exception,
        ISecretObfuscator secretObfuscator)
    {
        try
        {
            return secretObfuscator.Obfuscate(exception.Message, null);
        }
        catch (Exception)
        {
            return LoggingConstants.SecretMask;
        }
    }

    private static string GetObfuscatedText(
        Exception exception,
        ISecretObfuscator secretObfuscator)
    {
        try
        {
            return secretObfuscator.Obfuscate(exception.ToString(), null);
        }
        catch (Exception)
        {
            return $"{exception.GetType().FullName}: {GetObfuscatedMessage(exception, secretObfuscator)}";
        }
    }

    private static string? GetObfuscatedDiagnostic(
        Func<string?> getValue,
        ISecretObfuscator secretObfuscator)
    {
        try
        {
            return ObfuscateNullable(getValue(), secretObfuscator);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static void CopyDiagnostics(
        Exception destination,
        Exception source,
        ISecretObfuscator secretObfuscator)
    {
        TryCopyTargetSite(destination, source);
        destination.HResult = source.HResult;
        destination.HelpLink = GetObfuscatedDiagnostic(
            () => source.HelpLink,
            secretObfuscator);
        destination.Source = GetObfuscatedDiagnostic(
            () => source.Source,
            secretObfuscator);
        CopyData(destination, source, secretObfuscator);
    }

    private static void TryCopyTargetSite(Exception destination, Exception source)
    {
        if (!RuntimeFeature.IsDynamicCodeSupported)
        {
            TryCopyNativeAotTargetSiteState(destination, source);
            return;
        }

        try
        {
            GetExceptionMethod(destination) = GetTargetSite(source);
        }
        catch (MissingFieldException)
        {
            // The private runtime field is unavailable under some runtimes.
        }
    }

    private static void TryCopyNativeAotTargetSiteState(Exception destination, Exception source)
    {
        try
        {
            GetNativeAotStackTrace(destination) = GetNativeAotStackTrace(source)?.ToArray();
            GetNativeAotStackTraceCount(destination) = GetNativeAotStackTraceCount(source);
        }
        catch (MissingFieldException)
        {
            // The private runtime fields are unavailable under some runtimes.
        }
    }

    private static void CopyData(
        Exception destination,
        Exception source,
        ISecretObfuscator secretObfuscator)
    {
        try
        {
            foreach (DictionaryEntry entry in source.Data)
            {
                var key = SanitizeDataValue(entry.Key, secretObfuscator);
                if (key is null)
                {
                    continue;
                }

                destination.Data[GetUniqueDataKey(destination.Data, key)] =
                    SanitizeDataValue(entry.Value, secretObfuscator);
            }
        }
        catch (Exception)
        {
            // Diagnostic data must never make logging fail.
        }
    }

    private static object GetUniqueDataKey(IDictionary data, object key)
    {
        if (!data.Contains(key))
        {
            return key;
        }

        var baseKey = key.ToString() ?? string.Empty;
        for (var suffix = 2; ; suffix++)
        {
            var candidate = $"{baseKey} ({suffix})";
            if (!data.Contains(candidate))
            {
                return candidate;
            }
        }
    }

    private static object? SanitizeDataValue(
        object? value,
        ISecretObfuscator secretObfuscator)
    {
        if (value is null)
        {
            return null;
        }

        try
        {
            var text = value.ToString() ?? string.Empty;
            var obfuscated = secretObfuscator.Obfuscate(text, null);
            return obfuscated.Equals(text, StringComparison.Ordinal) ? value : obfuscated;
        }
        catch (Exception)
        {
            return LoggingConstants.SecretMask;
        }
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "TargetSite is copied as an optional diagnostic; missing trimmed metadata is acceptable.")]
    private static MethodBase? GetTargetSite(Exception exception) => exception.TargetSite;

    // TargetSite is non-virtual and has no public copy API; this is its .NET 10 backing field.
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_exceptionMethod")]
    private static extern ref MethodBase? GetExceptionMethod(Exception exception);

    // NativeAOT computes TargetSite from the first captured native stack trace entry.
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_corDbgStackTrace")]
    private static extern ref IntPtr[]? GetNativeAotStackTrace(Exception exception);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_idxFirstFreeStackTraceEntry")]
    private static extern ref int GetNativeAotStackTraceCount(Exception exception);

    private sealed class ObfuscatedAggregateLogException : AggregateException
    {
        private readonly string? _obfuscatedStackTrace;
        private readonly string _obfuscatedText;

        public ObfuscatedAggregateLogException(
            AggregateException exception,
            ISecretObfuscator secretObfuscator)
            : base(
                GetObfuscatedMessage(exception, secretObfuscator),
                exception.InnerExceptions.Select(inner => Create(inner, secretObfuscator)!))
        {
            _obfuscatedStackTrace = GetObfuscatedDiagnostic(
                () => exception.StackTrace,
                secretObfuscator);
            _obfuscatedText = GetObfuscatedText(exception, secretObfuscator);
            CopyDiagnostics(this, exception, secretObfuscator);
        }

        public override string? StackTrace => _obfuscatedStackTrace;

        public override string ToString() => _obfuscatedText;
    }
}
