using ModularPipelines.Engine;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides downstream loggers with an exception whose public text is safe to render.
/// </summary>
internal sealed class ObfuscatedLogException : Exception
{
    private readonly string _obfuscatedText;

    private ObfuscatedLogException(Exception exception, ISecretObfuscator secretObfuscator)
        : base(secretObfuscator.Obfuscate(exception.Message, null))
    {
        _obfuscatedText = secretObfuscator.Obfuscate(exception.ToString(), null);
        HResult = exception.HResult;
    }

    public static Exception? Create(Exception? exception, ISecretObfuscator secretObfuscator)
        => exception is null
            ? null
            : new ObfuscatedLogException(exception, secretObfuscator);

    public override string ToString() => _obfuscatedText;
}
