using System.Text;
using ModularPipelines.Engine;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.Logging;

/// <summary>
/// Obfuscates visible renderable text while retaining Spectre segment styling and control codes.
/// </summary>
internal sealed class SecretObfuscatedRenderable(
    IRenderable inner,
    ISecretObfuscator secretObfuscator) : IRenderable
{
    private readonly IRenderable _inner = PrepareCompositeLayout(inner, secretObfuscator);

    public Measurement Measure(RenderOptions options, int maxWidth) =>
        MeasureSegments(GetSegments(options, maxWidth), maxWidth);

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth) =>
        GetSegments(options, maxWidth);

    internal IRenderable Snapshot(RenderOptions options, int maxWidth) =>
        new SegmentSnapshotRenderable(GetSegments(options, maxWidth));

    private Segment[] GetSegments(RenderOptions options, int maxWidth)
    {
        var segments = _inner.Render(options, maxWidth).ToArray();
        var visibleText = string.Concat(
            segments.Where(static segment => !segment.IsControlCode).Select(static segment => segment.Text));

        if (visibleText.Length == 0)
        {
            return ObfuscateLinks(segments);
        }

        if (secretObfuscator is SecretObfuscator concreteObfuscator)
        {
            var preserveMasks = concreteObfuscator.CanSafelyPreserveRegisteredMasks();
            return MapSegments(
                segments,
                concreteObfuscator.ObfuscateWithSourceMap(visibleText, preserveMasks));
        }

        return MapFallbackSegments(
            segments,
            secretObfuscator.Obfuscate(visibleText, null),
            visibleText.Length);
    }

    private Segment[] MapSegments(
        Segment[] segments,
        SecretObfuscator.MappedObfuscatedOutput mappedOutput)
    {
        var output = new List<Segment>(segments.Length);
        var outputBytes = Encoding.UTF8.GetBytes(mappedOutput.Value);
        var sourceOffset = 0;
        foreach (var segment in segments)
        {
            if (segment.IsControlCode)
            {
                output.Add(segment);
                continue;
            }

            var segmentEnd = sourceOffset + segment.Text.Length;
            var outputStart = mappedOutput.SourceToOutputByteOffsets[sourceOffset];
            var outputEnd = mappedOutput.SourceToOutputByteOffsets[segmentEnd];
            sourceOffset = segmentEnd;

            if (outputEnd > outputStart)
            {
                output.Add(new Segment(
                    Encoding.UTF8.GetString(outputBytes, outputStart, outputEnd - outputStart),
                    segment.Style,
                    ObfuscateLink(segment.Link)));
            }
        }

        return [.. output];
    }

    private Segment[] MapFallbackSegments(
        Segment[] segments,
        string obfuscatedText,
        int sourceLength)
    {
        var output = new List<Segment>(segments.Length);
        var sourceOffset = 0;
        foreach (var segment in segments)
        {
            if (segment.IsControlCode)
            {
                output.Add(segment);
                continue;
            }

            var segmentEnd = sourceOffset + segment.Text.Length;
            var outputStart = ScaleOffset(sourceOffset, sourceLength, obfuscatedText);
            var outputEnd = ScaleOffset(segmentEnd, sourceLength, obfuscatedText);
            sourceOffset = segmentEnd;

            if (outputEnd > outputStart)
            {
                output.Add(new Segment(
                    obfuscatedText[outputStart..outputEnd],
                    segment.Style,
                    ObfuscateLink(segment.Link)));
            }
        }

        return [.. output];
    }

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

    private static IRenderable PrepareCompositeLayout(
        IRenderable renderable,
        ISecretObfuscator secretObfuscator)
    {
        if (renderable is not Table table)
        {
            return renderable;
        }

        var preparedTable = new Table
        {
            Border = table.Border,
            BorderStyle = table.BorderStyle,
            Caption = ObfuscateTitle(table.Caption, secretObfuscator),
            Expand = table.Expand,
            ShowFooters = table.ShowFooters,
            ShowHeaders = table.ShowHeaders,
            ShowRowSeparators = table.ShowRowSeparators,
            Title = ObfuscateTitle(table.Title, secretObfuscator),
            UseSafeBorder = table.UseSafeBorder,
            Width = table.Width,
        };

        foreach (var column in table.Columns)
        {
            preparedTable.AddColumn(new TableColumn(
                new SecretObfuscatedRenderable(column.Header, secretObfuscator))
            {
                Alignment = column.Alignment,
                Footer = column.Footer is null
                    ? null
                    : new SecretObfuscatedRenderable(column.Footer, secretObfuscator),
                NoWrap = column.NoWrap,
                Padding = column.Padding,
                Width = column.Width,
            });
        }

        foreach (var row in table.Rows)
        {
            preparedTable.Rows.Add(row.Select(
                cell => new SecretObfuscatedRenderable(cell, secretObfuscator)));
        }

        return preparedTable;
    }

    private static TableTitle? ObfuscateTitle(
        TableTitle? title,
        ISecretObfuscator secretObfuscator) => title is null
        ? null
        : new TableTitle(secretObfuscator.Obfuscate(title.Text, null), title.Style);

    private Segment[] ObfuscateLinks(Segment[] segments) =>
        [.. segments.Select(segment => segment.IsControlCode || segment.Link is null
            ? segment
            : new Segment(segment.Text, segment.Style, ObfuscateLink(segment.Link)))];

    private Link? ObfuscateLink(Link? link)
    {
        if (link is null)
        {
            return null;
        }

        var obfuscatedUrl = secretObfuscator.Obfuscate(link.Url, null);
        return string.Equals(obfuscatedUrl, link.Url, StringComparison.Ordinal)
            ? link
            : new Link(obfuscatedUrl);
    }

    private static Measurement MeasureSegments(Segment[] segments, int maxWidth)
    {
        var width = Segment.SplitLines(segments)
            .Select(static line => line.CellCount())
            .DefaultIfEmpty(0)
            .Max();
        width = Math.Min(width, maxWidth);
        return new Measurement(width, width);
    }

    private sealed class SegmentSnapshotRenderable(Segment[] segments) : IRenderable
    {
        public Measurement Measure(RenderOptions options, int maxWidth) =>
            MeasureSegments(segments, maxWidth);

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth) => segments;
    }
}
