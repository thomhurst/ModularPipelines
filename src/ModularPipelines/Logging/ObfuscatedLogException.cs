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
        HResult = exception.HResult;
        HelpLink = ObfuscateNullable(exception.HelpLink, secretObfuscator);
        Source = ObfuscateNullable(exception.Source, secretObfuscator);
    }

    public static Exception? Create(Exception? exception, ISecretObfuscator secretObfuscator)
        => exception is null
            ? null
            : new ObfuscatedLogException(exception, secretObfuscator);

    public override string? StackTrace => _obfuscatedStackTrace;

    public override string ToString() => _obfuscatedText;

    private static string? ObfuscateNullable(
        string? value,
        ISecretObfuscator secretObfuscator) =>
        value is null ? null : secretObfuscator.Obfuscate(value, null);
}
