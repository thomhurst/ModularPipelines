using System.Text;
using System.Text.RegularExpressions;
using ModularPipelines.Engine;
using Spectre.Console;

namespace ModularPipelines.Logging;

internal static partial class ObfuscatedMarkup
{
    internal static SecretObfuscatedRenderable Create(
        string value,
        ISecretObfuscator secretObfuscator)
    {
        var safeSource = CreateSafeSourceCore(value, secretObfuscator);
        if (safeSource.WasChanged)
        {
            return CreateRenderable(safeSource.Value, secretObfuscator);
        }

        var obfuscatedSource = secretObfuscator.Obfuscate(value, null);
        var tagObfuscatedSource = MarkupTagRegex().Replace(
            value,
            match => secretObfuscator.Obfuscate(match.Value, null));
        if (string.Equals(tagObfuscatedSource, obfuscatedSource, StringComparison.Ordinal))
        {
            return CreateRenderable(value, secretObfuscator);
        }

        return CreateRenderable(obfuscatedSource, secretObfuscator);
    }

    private static SecretObfuscatedRenderable CreateRenderable(
        string value,
        ISecretObfuscator secretObfuscator) =>
        SecretObfuscatedRenderable.FromPreObfuscated(new Markup(value), secretObfuscator);

    internal static string CreateSafeSource(
        string value,
        ISecretObfuscator secretObfuscator) =>
        CreateSafeSourceCore(value, secretObfuscator).Value;

    private static SafeSource CreateSafeSourceCore(
        string value,
        ISecretObfuscator secretObfuscator)
    {
        var matches = MarkupTagRegex().Matches(value).Cast<Match>().ToArray();
        var visibleText = GetVisibleText(value, matches);
        string obfuscatedText;
        Func<int, int, string> getObfuscatedSlice;
        if (secretObfuscator is SecretObfuscator concreteObfuscator)
        {
            var mappedOutput = concreteObfuscator.ObfuscateWithSourceMap(
                visibleText,
                concreteObfuscator.CanSafelyPreserveRegisteredMasks());
            var outputBytes = Encoding.UTF8.GetBytes(mappedOutput.Value);
            obfuscatedText = mappedOutput.Value;
            getObfuscatedSlice = (start, end) => Encoding.UTF8.GetString(
                outputBytes,
                mappedOutput.SourceToOutputByteOffsets[start],
                mappedOutput.SourceToOutputByteOffsets[end]
                - mappedOutput.SourceToOutputByteOffsets[start]);
        }
        else
        {
            obfuscatedText = secretObfuscator.Obfuscate(visibleText, null);
            getObfuscatedSlice = (start, end) => obfuscatedText[
                ScaleOffset(start, visibleText.Length, obfuscatedText)..
                ScaleOffset(end, visibleText.Length, obfuscatedText)];
        }

        var wasChanged = !string.Equals(visibleText, obfuscatedText, StringComparison.Ordinal);

        var output = new StringBuilder(value.Length);
        var sourceOffset = 0;
        var visibleOffset = 0;
        foreach (var match in matches)
        {
            var visibleFragment = DecodeEscapedBrackets(value[sourceOffset..match.Index]);
            var textLength = visibleFragment.Length;
            output.Append(Markup.Escape(getObfuscatedSlice(
                visibleOffset,
                visibleOffset + textLength)));
            output.Append(match.Value);
            sourceOffset = match.Index + match.Length;
            visibleOffset += textLength;
        }

        output.Append(Markup.Escape(getObfuscatedSlice(visibleOffset, visibleText.Length)));
        return new SafeSource(output.ToString(), wasChanged);
    }

    private static string GetVisibleText(string value, IReadOnlyList<Match> matches)
    {
        var output = new StringBuilder(value.Length);
        var sourceOffset = 0;
        foreach (var match in matches)
        {
            output.Append(DecodeEscapedBrackets(value[sourceOffset..match.Index]));
            sourceOffset = match.Index + match.Length;
        }

        output.Append(DecodeEscapedBrackets(value[sourceOffset..]));
        return output.ToString();
    }

    private static string DecodeEscapedBrackets(string value) => value
        .Replace("[[", "[", StringComparison.Ordinal)
        .Replace("]]", "]", StringComparison.Ordinal);

    private static int ScaleOffset(int sourceOffset, int sourceLength, string output)
    {
        if (sourceOffset >= sourceLength)
        {
            return output.Length;
        }

        var outputOffset = (int)((long) sourceOffset * output.Length / sourceLength);
        return outputOffset > 0
               && outputOffset < output.Length
               && char.IsLowSurrogate(output[outputOffset])
            ? outputOffset + 1
            : outputOffset;
    }

    private readonly record struct SafeSource(string Value, bool WasChanged);

    [GeneratedRegex(@"(?<!\[)\[(?!\[)[^\]\r\n]+\](?!\])", RegexOptions.CultureInvariant)]
    private static partial Regex MarkupTagRegex();
}
