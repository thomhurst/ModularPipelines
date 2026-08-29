using System.Text;
using ModularPipelines.Engine;
using Spectre.Console.Rendering;

namespace ModularPipelines.Logging;

/// <summary>
/// Obfuscates visible renderable text while retaining Spectre segment styling and control codes.
/// </summary>
internal sealed class SecretObfuscatedRenderable(
    IRenderable inner,
    ISecretObfuscator secretObfuscator) : IRenderable
{
    public Measurement Measure(RenderOptions options, int maxWidth) => inner.Measure(options, maxWidth);

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var segments = inner.Render(options, maxWidth).ToArray();
        var visibleText = string.Concat(
            segments.Where(static segment => !segment.IsControlCode).Select(static segment => segment.Text));

        if (visibleText.Length == 0)
        {
            return segments;
        }

        if (secretObfuscator is SecretObfuscator concreteObfuscator)
        {
            return MapSegments(
                segments,
                concreteObfuscator.ObfuscatePreservingMasksWithSourceMap(visibleText));
        }

        var obfuscated = secretObfuscator.Obfuscate(visibleText, null);
        return string.Equals(obfuscated, visibleText, StringComparison.Ordinal)
            ? segments
            : MapFallbackSegments(segments, obfuscated);
    }

    private static IEnumerable<Segment> MapSegments(
        IReadOnlyList<Segment> segments,
        SecretObfuscator.MappedObfuscatedOutput mappedOutput)
    {
        var outputBytes = Encoding.UTF8.GetBytes(mappedOutput.Value);
        var sourceOffset = 0;
        foreach (var segment in segments)
        {
            if (segment.IsControlCode)
            {
                yield return segment;
                continue;
            }

            var segmentEnd = sourceOffset + segment.Text.Length;
            var outputStart = mappedOutput.SourceToOutputByteOffsets[sourceOffset];
            var outputEnd = mappedOutput.SourceToOutputByteOffsets[segmentEnd];
            sourceOffset = segmentEnd;

            if (outputEnd > outputStart)
            {
                yield return new Segment(
                    Encoding.UTF8.GetString(outputBytes, outputStart, outputEnd - outputStart),
                    segment.Style,
                    segment.Link);
            }
        }
    }

    private static IEnumerable<Segment> MapFallbackSegments(
        IReadOnlyList<Segment> segments,
        string obfuscated)
    {
        var hasWrittenVisibleText = false;
        foreach (var segment in segments)
        {
            if (segment.IsControlCode)
            {
                yield return segment;
            }
            else if (!hasWrittenVisibleText)
            {
                hasWrittenVisibleText = true;
                yield return new Segment(obfuscated, segment.Style, segment.Link);
            }
        }
    }
}
