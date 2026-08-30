using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using ModularPipelines.Engine;
using ModularPipelines.Secrets;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.Logging;

/// <summary>
/// Obfuscates visible renderable text while retaining Spectre segment styling and safe control codes.
/// </summary>
internal sealed class SecretObfuscatedRenderable(
    IRenderable inner,
    ISecretObfuscator secretObfuscator,
    bool isObfuscatedBeforeRender = false) : IRenderable
{
    private readonly IRenderable _source = isObfuscatedBeforeRender
        ? inner
        : SnapshotRenderable(inner, secretObfuscator);

    internal bool RequiresPostRenderObfuscation => isObfuscatedBeforeRender;

    internal static SecretObfuscatedRenderable FromPreObfuscated(
        IRenderable renderable,
        ISecretObfuscator secretObfuscator) =>
        new(renderable, secretObfuscator, isObfuscatedBeforeRender: true);

    public Measurement Measure(RenderOptions options, int maxWidth)
    {
        var prepared = GetPrepared();
        var innerMeasurement = prepared.Renderable.Measure(options, maxWidth);
        var segments = GetSegments(prepared, options, maxWidth, out var originalSegments);
        return MeasureSegments(
            segments,
            originalSegments,
            options,
            maxWidth,
            innerMeasurement);
    }

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth) =>
        GetSegments(GetPrepared(), options, maxWidth);

    internal IRenderable Snapshot(RenderOptions options, int maxWidth)
    {
        var prepared = GetPrepared();
        var innerMeasurement = prepared.Renderable.Measure(options, maxWidth);
        var segments = GetSegments(prepared, options, maxWidth, out var originalSegments);
        return new SegmentSnapshotRenderable(segments, originalSegments, innerMeasurement);
    }

    private PreparedRenderable GetPrepared() => isObfuscatedBeforeRender
        ? Prepared(_source)
        : PrepareRenderable(_source, secretObfuscator);

    private Segment[] GetSegments(
        PreparedRenderable prepared,
        RenderOptions options,
        int maxWidth) =>
        GetSegments(prepared, options, maxWidth, out _);

    private Segment[] GetSegments(
        PreparedRenderable prepared,
        RenderOptions options,
        int maxWidth,
        out Segment[] originalSegments)
    {
        originalSegments = SanitizeControlCodes(
            prepared.Renderable.Render(options, maxWidth).ToArray());
        var segments = originalSegments;
        if (prepared.IsObfuscatedBeforeRender)
        {
            return ObfuscateLinks(segments);
        }

        var visibleText = string.Concat(
            segments.Where(static segment => !segment.IsControlCode).Select(static segment => segment.Text));

        if (visibleText.Length == 0)
        {
            return ObfuscateLinks(segments);
        }

        if (secretObfuscator is SecretObfuscator concreteObfuscator)
        {
            var preserveMasks = concreteObfuscator.CanSafelyPreserveRegisteredMasks();
            return ReflowSegments(
                MapSegments(
                    segments,
                    concreteObfuscator.ObfuscateWithSourceMap(visibleText, preserveMasks)),
                maxWidth);
        }

        return ReflowSegments(
            MapFallbackSegments(
                segments,
                secretObfuscator.Obfuscate(visibleText, null),
                visibleText.Length),
            maxWidth);
    }

    private static Segment[] ReflowSegments(Segment[] segments, int maxWidth)
    {
        if (maxWidth <= 0)
        {
            return [.. segments.Where(static segment => segment.IsControlCode)];
        }

        if (Segment.SplitLines(segments).All(line => line.CellCount() <= maxWidth))
        {
            return segments;
        }

        var lines = Segment.SplitLines(segments, maxWidth);
        var output = new List<Segment>(segments.Length + lines.Count - 1);
        for (var index = 0; index < lines.Count; index++)
        {
            output.AddRange(lines[index]);
            if (index < lines.Count - 1)
            {
                output.Add(Segment.LineBreak);
            }
        }

        return [.. output];
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

    private static PreparedRenderable PrepareRenderable(
        IRenderable renderable,
        ISecretObfuscator secretObfuscator,
        bool snapshot = false) => renderable switch
        {
            Align align => Prepared(PrepareAlign(align, secretObfuscator, snapshot)),
            BarChart barChart => Prepared(PrepareBarChart(barChart, secretObfuscator, snapshot)),
            BreakdownChart breakdownChart => Prepared(PrepareBreakdownChart(
                breakdownChart,
                secretObfuscator,
                snapshot)),
            Columns columns => Prepared(PrepareColumns(columns, secretObfuscator, snapshot)),
            FigletText figletText => Prepared(PrepareFigletText(figletText, secretObfuscator, snapshot)),
            Grid grid => Prepared(PrepareGrid(grid, secretObfuscator, snapshot)),
            Layout layout => Prepared(PrepareLayout(layout, secretObfuscator, snapshot)),
            Padder padder => Prepared(PreparePadder(padder, secretObfuscator, snapshot)),
            Panel panel => Prepared(PreparePanel(panel, secretObfuscator, snapshot)),
            Rule rule => Prepared(PrepareRule(rule, secretObfuscator, snapshot)),
            Rows rows => Prepared(PrepareRows(rows, secretObfuscator, snapshot)),
            Table table => Prepared(PrepareTable(table, secretObfuscator, snapshot)),
            Tree tree => Prepared(PrepareTree(tree, secretObfuscator, snapshot)),
            _ => new PreparedRenderable(renderable, IsObfuscatedBeforeRender: false),
        };

    private static IRenderable SnapshotRenderable(
        IRenderable renderable,
        ISecretObfuscator secretObfuscator) =>
        PrepareRenderable(renderable, secretObfuscator, snapshot: true).Renderable;

    private static IRenderable PrepareChild(
        IRenderable renderable,
        ISecretObfuscator secretObfuscator,
        bool snapshot) => snapshot
        ? SnapshotRenderable(renderable, secretObfuscator)
        : new SecretObfuscatedRenderable(renderable, secretObfuscator);

    private static string PrepareMarkup(
        string value,
        ISecretObfuscator secretObfuscator,
        bool snapshot) => snapshot
        ? value
        : ObfuscatedMarkup.CreateSafeSource(value, secretObfuscator);

    private static string PreparePlainText(
        string value,
        ISecretObfuscator secretObfuscator,
        bool snapshot) => snapshot
        ? value
        : secretObfuscator.Obfuscate(value, null);

    private static PreparedRenderable Prepared(IRenderable renderable) =>
        new(renderable, IsObfuscatedBeforeRender: true);

    private static Align PrepareAlign(
        Align align,
        ISecretObfuscator secretObfuscator,
        bool snapshot) =>
        new(PrepareChild(GetAlignChild(align), secretObfuscator, snapshot),
            align.Horizontal,
            align.Vertical)
        {
            Height = align.Height,
            Width = align.Width,
        };

    private static BarChart PrepareBarChart(
        BarChart barChart,
        ISecretObfuscator secretObfuscator,
        bool snapshot)
    {
        var preparedChart = new BarChart
        {
            Culture = barChart.Culture,
            Label = barChart.Label is null
                ? null
                : PrepareMarkup(barChart.Label, secretObfuscator, snapshot),
            LabelAlignment = barChart.LabelAlignment,
            MaxValue = barChart.MaxValue,
            ShowValues = barChart.ShowValues,
            ValueFormatter = PrepareValueFormatter(
                barChart.ValueFormatter,
                secretObfuscator,
                snapshot),
            Width = barChart.Width,
        };
        preparedChart.Data.AddRange(barChart.Data.Select(item => new BarChartItem(
            PrepareMarkup(item.Label, secretObfuscator, snapshot),
            item.Value,
            item.Color)));
        return preparedChart;
    }

    private static BreakdownChart PrepareBreakdownChart(
        BreakdownChart breakdownChart,
        ISecretObfuscator secretObfuscator,
        bool snapshot)
    {
        var preparedChart = new BreakdownChart
        {
            Compact = breakdownChart.Compact,
            Culture = breakdownChart.Culture,
            Expand = breakdownChart.Expand,
            ShowTags = breakdownChart.ShowTags,
            ShowTagValues = breakdownChart.ShowTagValues,
            ValueColor = breakdownChart.ValueColor,
            ValueFormatter = PrepareValueFormatter(
                breakdownChart.ValueFormatter,
                secretObfuscator,
                snapshot),
            Width = breakdownChart.Width,
        };
        preparedChart.Data.AddRange(breakdownChart.Data.Select(item => new BreakdownChartItem(
            PrepareMarkup(item.Label, secretObfuscator, snapshot),
            item.Value,
            item.Color)));
        return preparedChart;
    }

    private static Func<double, System.Globalization.CultureInfo, string>? PrepareValueFormatter(
        Func<double, System.Globalization.CultureInfo, string>? formatter,
        ISecretObfuscator secretObfuscator,
        bool snapshot) => formatter is null
        ? null
        : snapshot
            ? formatter
            : (value, culture) => PrepareMarkup(
                formatter(value, culture),
                secretObfuscator,
                snapshot: false);

    private static Columns PrepareColumns(
        Columns columns,
        ISecretObfuscator secretObfuscator,
        bool snapshot) => new(
        GetColumnItems(columns).Select(
            item => PrepareChild(item, secretObfuscator, snapshot)))
        {
            Expand = columns.Expand,
            Padding = columns.Padding,
        };

    private static FigletText PrepareFigletText(
        FigletText figletText,
        ISecretObfuscator secretObfuscator,
        bool snapshot) => new(
        GetFigletFont(figletText),
        PreparePlainText(GetFigletText(figletText), secretObfuscator, snapshot))
        {
            Color = figletText.Color,
            Justification = figletText.Justification,
            LayoutMode = figletText.LayoutMode,
            Pad = figletText.Pad,
        };

    private static Grid PrepareGrid(
        Grid grid,
        ISecretObfuscator secretObfuscator,
        bool snapshot)
    {
        var preparedGrid = new Grid
        {
            Expand = grid.Expand,
            Width = grid.Width,
        };
        foreach (var column in grid.Columns)
        {
            preparedGrid.AddColumn(new GridColumn
            {
                Alignment = column.Alignment,
                NoWrap = column.NoWrap,
                Padding = column.Padding,
                Width = column.Width,
            });
        }

        foreach (var row in grid.Rows)
        {
            preparedGrid.AddRow(row.Select(
                    cell => PrepareChild(cell, secretObfuscator, snapshot))
                .ToArray());
        }

        return preparedGrid;
    }

    private static Padder PreparePadder(
        Padder padder,
        ISecretObfuscator secretObfuscator,
        bool snapshot) => new(
        PrepareChild(GetPadderChild(padder), secretObfuscator, snapshot),
        padder.Padding)
        {
            Expand = padder.Expand,
        };

    private static Layout PrepareLayout(
        Layout layout,
        ISecretObfuscator secretObfuscator,
        bool snapshot)
    {
        var preparedLayout = new Layout
        {
            IsVisible = layout.IsVisible,
            Name = layout.Name is null
                ? null
                : PrepareMarkup(layout.Name, secretObfuscator, snapshot),
            Ratio = layout.Ratio,
            Size = layout.Size,
        };
        if (GetLayoutRenderable(layout) is { } renderable)
        {
            preparedLayout.Update(PrepareChild(renderable, secretObfuscator, snapshot));
        }

        if (layout.MinimumSize > 0)
        {
            preparedLayout.MinimumSize = layout.MinimumSize;
        }

        var children = GetLayoutChildren(layout)
            .Select(child => PrepareLayout(child, secretObfuscator, snapshot))
            .ToArray();

        if (children.Length == 0)
        {
            return preparedLayout;
        }

        return GetLayoutSplitter(layout).GetType().Name switch
        {
            "RowSplitter" => preparedLayout.SplitRows(children),
            "ColumnSplitter" => preparedLayout.SplitColumns(children),
            var splitter => throw new InvalidOperationException(
                $"Unsupported Spectre layout splitter '{splitter}'."),
        };
    }

    private static Panel PreparePanel(
        Panel panel,
        ISecretObfuscator secretObfuscator,
        bool snapshot) => new(
        PrepareChild(GetPanelChild(panel), secretObfuscator, snapshot))
        {
            Border = panel.Border,
            BorderStyle = panel.BorderStyle,
            Expand = panel.Expand,
            Header = ObfuscateHeader(panel.Header, secretObfuscator, snapshot),
            Height = panel.Height,
            Padding = panel.Padding,
            UseSafeBorder = panel.UseSafeBorder,
            Width = panel.Width,
        };

    private static Rows PrepareRows(
        Rows rows,
        ISecretObfuscator secretObfuscator,
        bool snapshot) => new(
        GetRowChildren(rows).Select(
            child => PrepareChild(child, secretObfuscator, snapshot)))
    {
        Expand = rows.Expand,
    };

    private static Rule PrepareRule(
        Rule rule,
        ISecretObfuscator secretObfuscator,
        bool snapshot) => new()
    {
        Border = rule.Border,
        Justification = rule.Justification,
        Style = rule.Style,
        Title = rule.Title is null
            ? null
            : PrepareMarkup(rule.Title, secretObfuscator, snapshot),
    };

    private static IRenderable PrepareTable(
        Table table,
        ISecretObfuscator secretObfuscator,
        bool snapshot)
    {
        var title = ObfuscateTitle(table.Title, secretObfuscator, snapshot);
        var caption = ObfuscateTitle(table.Caption, secretObfuscator, snapshot);
        var preparedTable = new Table
        {
            Border = table.Border,
            BorderStyle = table.BorderStyle,
            Caption = caption,
            Expand = table.Expand,
            ShowFooters = table.ShowFooters,
            ShowHeaders = table.ShowHeaders,
            ShowRowSeparators = table.ShowRowSeparators,
            Title = title,
            UseSafeBorder = table.UseSafeBorder,
            Width = table.Width,
        };

        foreach (var column in table.Columns)
        {
            preparedTable.AddColumn(new TableColumn(
                PrepareChild(column.Header, secretObfuscator, snapshot))
            {
                Alignment = column.Alignment,
                Footer = column.Footer is null
                    ? null
                    : PrepareChild(column.Footer, secretObfuscator, snapshot),
                NoWrap = column.NoWrap,
                Padding = column.Padding,
                Width = column.Width,
            });
        }

        foreach (var row in table.Rows)
        {
            preparedTable.Rows.Add(row.Select(
                cell => PrepareChild(cell, secretObfuscator, snapshot)));
        }

        var titleWidth = GetTitleWidth(title, caption);
        return !snapshot && table.Width is null && titleWidth is not null
            ? new AutoSizedTable(preparedTable, titleWidth.Value)
            : preparedTable;
    }

    private static int? GetTitleWidth(TableTitle? title, TableTitle? caption)
    {
        var contentWidth = new[] { title, caption }
            .Where(static item => item is not null)
            .Select(static item => new Segment(Markup.Remove(item!.Text)).CellCount())
            .DefaultIfEmpty(0)
            .Max();
        return contentWidth == 0 ? null : contentWidth + 2;
    }

    private static Tree PrepareTree(
        Tree tree,
        ISecretObfuscator secretObfuscator,
        bool snapshot)
    {
        var root = GetTreeRoot(tree);
        var preparedTree = new Tree(PrepareChild(
            GetTreeNodeRenderable(root),
            secretObfuscator,
            snapshot))
        {
            Expanded = tree.Expanded,
            Guide = tree.Guide,
            Style = tree.Style,
        };
        preparedTree.Nodes.AddRange(root.Nodes.Select(
            node => PrepareTreeNode(node, secretObfuscator, snapshot)));
        return preparedTree;
    }

    private static TreeNode PrepareTreeNode(
        TreeNode node,
        ISecretObfuscator secretObfuscator,
        bool snapshot)
    {
        var preparedNode = new TreeNode(PrepareChild(
            GetTreeNodeRenderable(node),
            secretObfuscator,
            snapshot))
        {
            Expanded = node.Expanded,
        };
        preparedNode.Nodes.AddRange(node.Nodes.Select(
            child => PrepareTreeNode(child, secretObfuscator, snapshot)));
        return preparedNode;
    }

    private static TableTitle? ObfuscateTitle(
        TableTitle? title,
        ISecretObfuscator secretObfuscator,
        bool snapshot) => title is null
        ? null
        : new TableTitle(
            PrepareMarkup(title.Text, secretObfuscator, snapshot),
            title.Style);

    private static PanelHeader? ObfuscateHeader(
        PanelHeader? header,
        ISecretObfuscator secretObfuscator,
        bool snapshot) => header is null
        ? null
        : new PanelHeader(
            PrepareMarkup(header.Text, secretObfuscator, snapshot),
            header.Justification);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_renderable")]
    private static extern ref readonly IRenderable GetAlignChild(Align align);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_items")]
    private static extern ref readonly List<IRenderable> GetColumnItems(Columns columns);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_font")]
    private static extern ref readonly FigletFont GetFigletFont(FigletText figletText);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_text")]
    private static extern ref readonly string GetFigletText(FigletText figletText);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_child")]
    private static extern ref readonly IRenderable GetPadderChild(Padder padder);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_child")]
    private static extern ref readonly IRenderable GetPanelChild(Panel panel);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_children")]
    private static extern ref readonly Layout[] GetLayoutChildren(Layout layout);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_renderable")]
    private static extern ref readonly IRenderable? GetLayoutRenderable(Layout layout);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_children")]
    private static extern ref readonly List<IRenderable> GetRowChildren(Rows rows);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_root")]
    private static extern ref readonly TreeNode GetTreeRoot(Tree tree);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_Renderable")]
    private static extern IRenderable GetTreeNodeRenderable(TreeNode node);

    private static object GetLayoutSplitter(Layout layout) =>
        LayoutSplitterField.GetValue(layout)
        ?? throw new InvalidOperationException("Spectre layout has no splitter.");

    private static readonly FieldInfo LayoutSplitterField = typeof(Layout).GetField(
        "_splitter",
        BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new MissingFieldException(typeof(Layout).FullName, "_splitter");

    private Segment[] ObfuscateLinks(Segment[] segments)
    {
        var output = new List<Segment>(segments.Length);
        foreach (var segment in segments)
        {
            if (segment.IsControlCode)
            {
                output.Add(segment);
            }
            else
            {
                output.Add(segment.Link is null
                    ? segment
                    : new Segment(segment.Text, segment.Style, ObfuscateLink(segment.Link)));
            }
        }

        return [.. output];
    }

    private Segment[] SanitizeControlCodes(Segment[] segments) =>
        segments.Any(segment => segment.IsControlCode && !IsSafeControlCode(segment))
            ? [.. segments.Where(static segment => !segment.IsControlCode)]
            : segments;

    private bool IsSafeControlCode(Segment segment) =>
        string.Equals(
            ObfuscateMetadata(segment.Text),
            segment.Text,
            StringComparison.Ordinal);

    private Link? ObfuscateLink(Link? link)
    {
        if (link is null)
        {
            return null;
        }

        var obfuscatedUrl = ObfuscateMetadata(link.Url);
        return string.Equals(obfuscatedUrl, link.Url, StringComparison.Ordinal)
            ? link
            : new Link(obfuscatedUrl);
    }

    private string ObfuscateMetadata(string value) =>
        secretObfuscator is SecretObfuscator concreteObfuscator
            ? concreteObfuscator.ObfuscateWithSourceMap(
                value,
                concreteObfuscator.CanSafelyPreserveRegisteredMasks()).Value
            : secretObfuscator.Obfuscate(value, null);

    private static Measurement MeasureSegments(
        Segment[] segments,
        Segment[] originalSegments,
        RenderOptions options,
        int maxWidth,
        Measurement innerMeasurement)
    {
        var width = Segment.SplitLines(segments)
            .Select(static line => line.CellCount())
            .DefaultIfEmpty(0)
            .Max();
        width = Math.Min(width, maxWidth);
        var innerMaximumWidth = Math.Min(innerMeasurement.Max, maxWidth);
        var innerMinimumWidth = Math.Min(innerMeasurement.Min, innerMaximumWidth);
        var originalText = GetVisibleText(originalSegments);
        var transformedText = GetVisibleText(segments);
        var minimumWidth = string.Equals(originalText, transformedText, StringComparison.Ordinal)
            ? Math.Min(innerMinimumWidth, width)
            : GetTransformedMinimumWidth(
                originalText,
                transformedText,
                options,
                maxWidth,
                innerMinimumWidth,
                width);
        return new Measurement(minimumWidth, width);
    }

    private static string GetVisibleText(IEnumerable<Segment> segments) =>
        string.Concat(segments
            .Where(static segment => !segment.IsControlCode)
            .Select(static segment => segment.Text));

    private static int GetTransformedMinimumWidth(
        string originalText,
        string transformedText,
        RenderOptions options,
        int maxWidth,
        int innerMinimumWidth,
        int transformedMaximumWidth)
    {
        var originalTextMinimumWidth = ((IRenderable) new Text(originalText))
            .Measure(options, maxWidth).Min;
        var transformedTextMinimumWidth = ((IRenderable) new Text(transformedText))
            .Measure(options, maxWidth).Min;
        var structuralMinimumWidth = Math.Max(0, innerMinimumWidth - originalTextMinimumWidth);
        return Math.Clamp(
            structuralMinimumWidth + transformedTextMinimumWidth,
            0,
            transformedMaximumWidth);
    }

    private sealed class SegmentSnapshotRenderable(
        Segment[] segments,
        Segment[] originalSegments,
        Measurement innerMeasurement) : IRenderable
    {
        public Measurement Measure(RenderOptions options, int maxWidth) =>
            MeasureSegments(segments, originalSegments, options, maxWidth, innerMeasurement);

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth) => segments;
    }

    private readonly record struct PreparedRenderable(
        IRenderable Renderable,
        bool IsObfuscatedBeforeRender);

    private sealed class AutoSizedTable(Table table, int minimumWidth) : IRenderable
    {
        private readonly object _renderLock = new();

        public Measurement Measure(RenderOptions options, int maxWidth)
        {
            lock (_renderLock)
            {
                SetWidth(options, maxWidth);
                return ((IRenderable) table).Measure(options, maxWidth);
            }
        }

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
        {
            lock (_renderLock)
            {
                SetWidth(options, maxWidth);
                return ((IRenderable) table).Render(options, maxWidth).ToArray();
            }
        }

        private void SetWidth(RenderOptions options, int maxWidth)
        {
            table.Width = null;
            var contentWidth = ((IRenderable) table).Measure(options, maxWidth).Max;
            table.Width = Math.Min(maxWidth, Math.Max(contentWidth, minimumWidth));
        }
    }
}
