using System.Text;
using ModularPipelines.Logging;
using ModularPipelines.Secrets;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.Console;

/// <summary>
/// Groups buffered fragments across secret boundaries and remasks them immediately before emission.
/// </summary>
internal sealed class BufferedSecretRemasker(
    ISecretObfuscator? secretObfuscator,
    ISecretProvider? secretProvider)
{
    private readonly ISecretObfuscator? _secretObfuscator = secretObfuscator;
    private readonly ISecretProvider? _secretProvider = secretProvider;

    public int GetIncrementalFlushableOutputCount(IReadOnlyList<BufferedOutput> outputs)
    {
        if (_secretObfuscator is null)
        {
            return outputs.Count;
        }

        var count = outputs.Count;
        while (count > 0
               && outputs[count - 1].IsMaskable
               && (!outputs[count - 1].AppendNewLine
                   || HasPotentialSecretAtIncrementalFlushBoundary(outputs, count)))
        {
            count--;
        }

        return count;
    }

    public int TryWrite(
        IAnsiConsole console,
        IReadOnlyList<BufferedOutput> outputs,
        int index)
    {
        if (_secretObfuscator is null || !outputs[index].IsMaskable)
        {
            return 0;
        }

        var maskableOutputs = GetMaskableOutputs(outputs, index);
        Write(console, maskableOutputs);
        return maskableOutputs.Length;
    }

    public void WriteRenderable(
        IAnsiConsole console,
        IRenderable renderable,
        ISecretObfuscator? outputObfuscator = null)
    {
        outputObfuscator ??= _secretObfuscator;
        if (outputObfuscator is null)
        {
            console.Write(renderable);
            return;
        }

        var remasked = renderable is SecretObfuscatedRenderable
        {
            RequiresPostRenderObfuscation: false,
        }
            ? renderable
            : new SecretObfuscatedRenderable(renderable, outputObfuscator);
        if (_secretProvider is ISecretEmissionGuard emissionGuard)
        {
            emissionGuard.ExecuteWithStableSecrets(
                (Console: console, Renderable: remasked),
                static state => state.Console.Write(state.Renderable));
            return;
        }

        console.Write(remasked);
    }

    private BufferedOutput[] GetMaskableOutputs(
        IReadOnlyList<BufferedOutput> outputs,
        int firstIndex)
    {
        var lastIndex = firstIndex;
        while (lastIndex + 1 < outputs.Count && outputs[lastIndex + 1].IsMaskable)
        {
            if (outputs[lastIndex].AppendNewLine
                && !HasPotentialSecretAcrossLineBoundary(outputs, firstIndex, lastIndex))
            {
                break;
            }

            lastIndex++;
        }

        return [.. outputs.Skip(firstIndex).Take(lastIndex - firstIndex + 1)];
    }

    private void Write(IAnsiConsole console, BufferedOutput[] outputs)
    {
        if (outputs.All(static output => output is { IsRenderable: false, IsPreObfuscated: true }))
        {
            WritePreObfuscatedStringsWithCurrentSecrets(console, outputs);
            return;
        }

        if (outputs.All(static output => output is { IsRenderable: false, IsPreObfuscated: false }))
        {
            WriteRawStringsWithCurrentSecrets(console, GetSource(outputs));
            WriteTrailingNewLine(console, outputs);
            return;
        }

        var renderables = GetRenderables(outputs);
        var renderable = renderables.Count == 1
            ? renderables[0]
            : new ConcatenatedRenderable(renderables);
        WriteRenderable(console, renderable, GetCombinedOutputObfuscator(outputs.Length));
        WriteTrailingNewLine(console, outputs);
    }

    private bool HasPotentialSecretAtIncrementalFlushBoundary(
        IReadOnlyList<BufferedOutput> outputs,
        int outputCount)
    {
        var firstMaskableIndex = outputCount - 1;
        while (firstMaskableIndex > 0 && outputs[firstMaskableIndex - 1].IsMaskable)
        {
            firstMaskableIndex--;
        }

        var source = GetSource(outputs, firstMaskableIndex, outputCount, includeTrailingNewLine: true);

        // ModuleOutputBuffer adds a blank separator after every visible incremental group.
        return GetPotentialSecretPrefixLength(source + Environment.NewLine) > 0;
    }

    private bool HasPotentialSecretAcrossLineBoundary(
        IReadOnlyList<BufferedOutput> outputs,
        int firstIndex,
        int lastIndex)
    {
        if (_secretProvider is null)
        {
            return false;
        }

        var source = GetSource(outputs, firstIndex, lastIndex + 1, includeTrailingNewLine: true);
        return GetPotentialSecretPrefixLength(source) > 0;
    }

    private ISecretObfuscator? GetCombinedOutputObfuscator(int outputCount)
    {
        if (_secretObfuscator is null
            || _secretObfuscator is SecretObfuscator
            || _secretProvider is null
            || outputCount == 1)
        {
            return _secretObfuscator;
        }

        return new RegisteredSecretsOnlyObfuscator(_secretObfuscator, _secretProvider);
    }

    private int GetPotentialSecretPrefixLength(string value)
    {
        if (_secretProvider is null || value.Length == 0)
        {
            return 0;
        }

        var comparison = _secretObfuscator is ITrackedSecretObfuscator tracked
            ? tracked.PatternComparison
            : StringComparison.OrdinalIgnoreCase;
        var retainedLength = 0;
        var secrets = _secretProvider.GetSnapshot().Secrets ?? [];
        foreach (var secret in secrets.Where(static secret => !string.IsNullOrEmpty(secret)))
        {
            var maximumLength = Math.Min(value.Length, secret.Length - 1);
            for (var length = maximumLength; length > 0; length--)
            {
                if (secret.AsSpan().StartsWith(value.AsSpan(value.Length - length), comparison))
                {
                    retainedLength = Math.Max(retainedLength, length);
                    break;
                }
            }
        }

        return retainedLength;
    }

    private void WriteRawStringsWithCurrentSecrets(IAnsiConsole console, string source)
    {
        if (_secretObfuscator is null)
        {
            console.Markup(source);
            return;
        }

        if (_secretProvider is ISecretEmissionGuard emissionGuard)
        {
            emissionGuard.ExecuteWithStableSecrets(
                (Console: console, Source: source, Obfuscator: _secretObfuscator),
                static state => state.Console.Write(
                    ObfuscatedMarkup.Create(state.Source, state.Obfuscator)));
            return;
        }

        console.Write(ObfuscatedMarkup.Create(source, _secretObfuscator));
    }

    private void WritePreObfuscatedStringsWithCurrentSecrets(
        IAnsiConsole console,
        BufferedOutput[] outputs)
    {
        if (_secretObfuscator is null || _secretProvider is null)
        {
            WritePreObfuscatedStrings(console, outputs);
            return;
        }

        if (_secretProvider is ISecretEmissionGuard emissionGuard)
        {
            emissionGuard.ExecuteWithStableSecrets(
                (Remasker: this, Console: console, Outputs: outputs),
                static state => state.Remasker.WritePreObfuscatedStrings(
                    state.Console,
                    state.Outputs));
            return;
        }

        WritePreObfuscatedStrings(console, outputs);
    }

    private void WritePreObfuscatedStrings(IAnsiConsole console, BufferedOutput[] outputs)
    {
        console.Write(new Text(RemaskCurrentSecrets(GetSource(outputs))));
        WriteTrailingNewLine(console, outputs);
    }

    private string RemaskCurrentSecrets(string source)
    {
        if (_secretObfuscator is null || _secretProvider is null)
        {
            return source;
        }

        var comparison = _secretObfuscator is ITrackedSecretObfuscator tracked
            ? tracked.PatternComparison
            : StringComparison.OrdinalIgnoreCase;
        var comparer = comparison == StringComparison.Ordinal
            ? StringComparer.Ordinal
            : StringComparer.OrdinalIgnoreCase;
        var secrets = (_secretProvider.GetSnapshot().Secrets ?? [])
            .Where(static secret => !string.IsNullOrEmpty(secret))
            .Distinct(comparer)
            .OrderByDescending(static secret => secret.Length)
            .ToArray();
        if (secrets.Length == 0)
        {
            return source;
        }

        var output = new StringBuilder(source.Length);
        for (var offset = 0; offset < source.Length;)
        {
            var matchedSecret = secrets.FirstOrDefault(secret =>
                source.AsSpan(offset).StartsWith(secret.AsSpan(), comparison));
            if (matchedSecret is null)
            {
                output.Append(source[offset]);
                offset++;
                continue;
            }

            output.Append(_secretObfuscator.Obfuscate(
                source.Substring(offset, matchedSecret.Length),
                null));
            offset += matchedSecret.Length;
        }

        return output.ToString();
    }

    private static List<IRenderable> GetRenderables(BufferedOutput[] outputs)
    {
        var renderables = new List<IRenderable>((outputs.Length * 2) - 1);
        for (var index = 0; index < outputs.Length; index++)
        {
            var output = outputs[index];
            renderables.Add(GetRenderable(output));
            if (output.AppendNewLine && index < outputs.Length - 1)
            {
                renderables.Add(new Text(Environment.NewLine));
            }
        }

        return renderables;
    }

    private static IRenderable GetRenderable(BufferedOutput output) =>
        output.Renderable
        ?? (output.StringValue is { } value
            ? new BufferedStringRenderable(value)
            : throw new InvalidOperationException("Buffered output is not maskable."));

    private static string GetSource(BufferedOutput[] outputs) =>
        GetSource(outputs, 0, outputs.Length, includeTrailingNewLine: false);

    private static string GetSource(
        IReadOnlyList<BufferedOutput> outputs,
        int firstIndex,
        int exclusiveEndIndex,
        bool includeTrailingNewLine)
    {
        var source = new StringBuilder();
        for (var index = firstIndex; index < exclusiveEndIndex; index++)
        {
            source.Append(outputs[index].MaskablePlainText);
            if (outputs[index].AppendNewLine
                && (includeTrailingNewLine || index < exclusiveEndIndex - 1))
            {
                source.Append(Environment.NewLine);
            }
        }

        return source.ToString();
    }

    private static void WriteTrailingNewLine(IAnsiConsole console, BufferedOutput[] outputs)
    {
        if (outputs[^1].AppendNewLine)
        {
            console.WriteLine();
        }
    }

    private sealed class ConcatenatedRenderable(IReadOnlyList<IRenderable> renderables) : IRenderable
    {
        public Measurement Measure(RenderOptions options, int maxWidth)
        {
            var text = string.Concat(Render(options, maxWidth)
                .Where(static segment => !segment.IsControlCode)
                .Select(static segment => segment.Text));
            return ((IRenderable) new Text(text)).Measure(options, maxWidth);
        }

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth) =>
            renderables.SelectMany(renderable => renderable.Render(options, maxWidth));
    }

    private sealed class BufferedStringRenderable(string value) : IRenderable
    {
        public Measurement Measure(RenderOptions options, int maxWidth)
        {
            try
            {
                return ((IRenderable) new Markup(value)).Measure(options, maxWidth);
            }
            catch (Exception)
            {
                return ((IRenderable) new Text(value)).Measure(options, maxWidth);
            }
        }

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
        {
            try
            {
                return [.. ((IRenderable) new Markup(value)).Render(options, maxWidth)];
            }
            catch (Exception)
            {
                return [.. ((IRenderable) new Text(value)).Render(options, maxWidth)];
            }
        }
    }

    private sealed class RegisteredSecretsOnlyObfuscator(
        ISecretObfuscator inner,
        ISecretProvider secretProvider) : ISecretObfuscator
    {
        private readonly BufferedSecretRemasker _remasker = new(inner, secretProvider);

        public bool HasSecrets => inner.HasSecrets;

        public string Obfuscate(string? input, object? optionsObject) =>
            _remasker.RemaskCurrentSecrets(input ?? string.Empty);
    }
}
