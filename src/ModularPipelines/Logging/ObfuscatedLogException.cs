using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
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
            secretObfuscator.Obfuscate(exception.Message, null),
            Create(exception.InnerException, secretObfuscator))
    {
        _obfuscatedStackTrace = ObfuscateNullable(exception.StackTrace, secretObfuscator);
        _obfuscatedText = secretObfuscator.Obfuscate(exception.ToString(), null);
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

    private static void CopyDiagnostics(
        Exception destination,
        Exception source,
        ISecretObfuscator secretObfuscator)
    {
        GetExceptionMethod(destination) = GetTargetSite(source);
        destination.HResult = source.HResult;
        destination.HelpLink = ObfuscateNullable(source.HelpLink, secretObfuscator);
        destination.Source = ObfuscateNullable(source.Source, secretObfuscator);
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "TargetSite is copied as an optional diagnostic; missing trimmed metadata is acceptable.")]
    private static MethodBase? GetTargetSite(Exception exception) => exception.TargetSite;

    // TargetSite is non-virtual and has no public copy API; this is its .NET 10 backing field.
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_exceptionMethod")]
    private static extern ref MethodBase? GetExceptionMethod(Exception exception);

    private sealed class ObfuscatedAggregateLogException : AggregateException
    {
        private readonly string? _obfuscatedStackTrace;
        private readonly string _obfuscatedText;

        public ObfuscatedAggregateLogException(
            AggregateException exception,
            ISecretObfuscator secretObfuscator)
            : base(
                secretObfuscator.Obfuscate(exception.Message, null),
                exception.InnerExceptions.Select(inner => Create(inner, secretObfuscator)!))
        {
            _obfuscatedStackTrace = ObfuscateNullable(exception.StackTrace, secretObfuscator);
            _obfuscatedText = secretObfuscator.Obfuscate(exception.ToString(), null);
            CopyDiagnostics(this, exception, secretObfuscator);
        }

        public override string? StackTrace => _obfuscatedStackTrace;

        public override string ToString() => _obfuscatedText;
    }
}
