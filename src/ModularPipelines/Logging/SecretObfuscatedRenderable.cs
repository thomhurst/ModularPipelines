using System.Reflection;
using System.Runtime.CompilerServices;
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
    private readonly PreparedRenderable _prepared = PrepareRenderable(inner, secretObfuscator);

    public Measurement Measure(RenderOptions options, int maxWidth)
    {
        var innerMeasurement = _prepared.Renderable.Measure(options, maxWidth);
        return MeasureSegments(GetSegments(options, maxWidth), maxWidth, innerMeasurement);
    }

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth) =>
        GetSegments(options, maxWidth);

    internal IRenderable Snapshot(RenderOptions options, int maxWidth)
    {
        var innerMeasurement = _prepared.Renderable.Measure(options, maxWidth);
        return new SegmentSnapshotRenderable(GetSegments(options, maxWidth), innerMeasurement);
    }

    private Segment[] GetSegments(RenderOptions options, int maxWidth)
    {
        var segments = _prepared.Renderable.Render(options, maxWidth).ToArray();
        if (_prepared.IsObfuscatedBeforeRender)
        {
            return segments;
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
                output.Add(ObfuscateControlCode(segment));
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
                output.Add(ObfuscateControlCode(segment));
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
        ISecretObfuscator secretObfuscator) => renderable switch
        {
            Align align => Prepared(PrepareAlign(align, secretObfuscator)),
            BarChart barChart => Prepared(PrepareBarChart(barChart, secretObfuscator)),
            Columns columns => Prepared(PrepareColumns(columns, secretObfuscator)),
            FigletText figletText => Prepared(PrepareFigletText(figletText, secretObfuscator)),
            Grid grid => Prepared(PrepareGrid(grid, secretObfuscator)),
            Layout layout => Prepared(PrepareLayout(layout, secretObfuscator)),
            Padder padder => Prepared(PreparePadder(padder, secretObfuscator)),
            Panel panel => Prepared(PreparePanel(panel, secretObfuscator)),
            Rule rule => Prepared(PrepareRule(rule, secretObfuscator)),
            Rows rows => Prepared(PrepareRows(rows, secretObfuscator)),
            Table table => Prepared(PrepareTable(table, secretObfuscator)),
            Tree tree => Prepared(PrepareTree(tree, secretObfuscator)),
            _ => new PreparedRenderable(renderable, IsObfuscatedBeforeRender: false),
        };

    private static PreparedRenderable Prepared(IRenderable renderable) =>
        new(renderable, IsObfuscatedBeforeRender: true);

    private static Align PrepareAlign(Align align, ISecretObfuscator secretObfuscator) =>
        new(new SecretObfuscatedRenderable(GetAlignChild(align), secretObfuscator),
            align.Horizontal,
            align.Vertical)
        {
            Height = align.Height,
            Width = align.Width,
        };

    private static BarChart PrepareBarChart(
        BarChart barChart,
        ISecretObfuscator secretObfuscator)
    {
        var preparedChart = new BarChart
        {
            Culture = barChart.Culture,
            Label = barChart.Label is null
                ? null
                : ObfuscatedMarkup.CreateSafeSource(barChart.Label, secretObfuscator),
            LabelAlignment = barChart.LabelAlignment,
            MaxValue = barChart.MaxValue,
            ShowValues = barChart.ShowValues,
            ValueFormatter = PrepareValueFormatter(barChart.ValueFormatter, secretObfuscator),
            Width = barChart.Width,
        };
        preparedChart.Data.AddRange(barChart.Data.Select(item => new BarChartItem(
            ObfuscatedMarkup.CreateSafeSource(item.Label, secretObfuscator),
            item.Value,
            item.Color)));
        return preparedChart;
    }

    private static Func<double, System.Globalization.CultureInfo, string>? PrepareValueFormatter(
        Func<double, System.Globalization.CultureInfo, string>? formatter,
        ISecretObfuscator secretObfuscator) => formatter is null
        ? null
        : (value, culture) => ObfuscatedMarkup.CreateSafeSource(
            formatter(value, culture),
            secretObfuscator);

    private static Columns PrepareColumns(
        Columns columns,
        ISecretObfuscator secretObfuscator) => new(
        GetColumnItems(columns).Select(
            item => new SecretObfuscatedRenderable(item, secretObfuscator)))
        {
            Expand = columns.Expand,
            Padding = columns.Padding,
        };

    private static FigletText PrepareFigletText(
        FigletText figletText,
        ISecretObfuscator secretObfuscator) => new(
        GetFigletFont(figletText),
        secretObfuscator.Obfuscate(GetFigletText(figletText), null))
        {
            Color = figletText.Color,
            Justification = figletText.Justification,
            LayoutMode = figletText.LayoutMode,
            Pad = figletText.Pad,
        };

    private static Grid PrepareGrid(Grid grid, ISecretObfuscator secretObfuscator)
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
                    cell => new SecretObfuscatedRenderable(cell, secretObfuscator))
                .ToArray());
        }

        return preparedGrid;
    }

    private static Padder PreparePadder(
        Padder padder,
        ISecretObfuscator secretObfuscator) => new(
        new SecretObfuscatedRenderable(GetPadderChild(padder), secretObfuscator),
        padder.Padding)
        {
            Expand = padder.Expand,
        };

    private static Layout PrepareLayout(
        Layout layout,
        ISecretObfuscator secretObfuscator)
    {
        var preparedLayout = new Layout
        {
            IsVisible = layout.IsVisible,
            Name = layout.Name is null
                ? null
                : ObfuscatedMarkup.CreateSafeSource(layout.Name, secretObfuscator),
            Ratio = layout.Ratio,
            Size = layout.Size,
        };
        if (GetLayoutRenderable(layout) is { } renderable)
        {
            preparedLayout.Update(new SecretObfuscatedRenderable(renderable, secretObfuscator));
        }

        if (layout.MinimumSize > 0)
        {
            preparedLayout.MinimumSize = layout.MinimumSize;
        }

        var children = GetLayoutChildren(layout)
            .Select(child => PrepareLayout(child, secretObfuscator))
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
        ISecretObfuscator secretObfuscator) => new(
        new SecretObfuscatedRenderable(GetPanelChild(panel), secretObfuscator))
        {
            Border = panel.Border,
            BorderStyle = panel.BorderStyle,
            Expand = panel.Expand,
            Header = ObfuscateHeader(panel.Header, secretObfuscator),
            Height = panel.Height,
            Padding = panel.Padding,
            UseSafeBorder = panel.UseSafeBorder,
            Width = panel.Width,
        };

    private static Rows PrepareRows(Rows rows, ISecretObfuscator secretObfuscator) => new(
        GetRowChildren(rows).Select(
            child => new SecretObfuscatedRenderable(child, secretObfuscator)))
    {
        Expand = rows.Expand,
    };

    private static Rule PrepareRule(Rule rule, ISecretObfuscator secretObfuscator) => new()
    {
        Border = rule.Border,
        Justification = rule.Justification,
        Style = rule.Style,
        Title = rule.Title is null
            ? null
            : ObfuscatedMarkup.CreateSafeSource(rule.Title, secretObfuscator),
    };

    private static IRenderable PrepareTable(Table table, ISecretObfuscator secretObfuscator)
    {
        var title = ObfuscateTitle(table.Title, secretObfuscator);
        var caption = ObfuscateTitle(table.Caption, secretObfuscator);
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

        var titleWidth = GetTitleWidth(title, caption);
        return table.Width is null && titleWidth is not null
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

    private static Tree PrepareTree(Tree tree, ISecretObfuscator secretObfuscator)
    {
        var root = GetTreeRoot(tree);
        var preparedTree = new Tree(new SecretObfuscatedRenderable(
            GetTreeNodeRenderable(root),
            secretObfuscator))
        {
            Expanded = tree.Expanded,
            Guide = tree.Guide,
            Style = tree.Style,
        };
        preparedTree.Nodes.AddRange(root.Nodes.Select(
            node => PrepareTreeNode(node, secretObfuscator)));
        return preparedTree;
    }

    private static TreeNode PrepareTreeNode(
        TreeNode node,
        ISecretObfuscator secretObfuscator)
    {
        var preparedNode = new TreeNode(new SecretObfuscatedRenderable(
            GetTreeNodeRenderable(node),
            secretObfuscator))
        {
            Expanded = node.Expanded,
        };
        preparedNode.Nodes.AddRange(node.Nodes.Select(
            child => PrepareTreeNode(child, secretObfuscator)));
        return preparedNode;
    }

    private static TableTitle? ObfuscateTitle(
        TableTitle? title,
        ISecretObfuscator secretObfuscator) => title is null
        ? null
        : new TableTitle(
            ObfuscatedMarkup.CreateSafeSource(title.Text, secretObfuscator),
            title.Style);

    private static PanelHeader? ObfuscateHeader(
        PanelHeader? header,
        ISecretObfuscator secretObfuscator) => header is null
        ? null
        : new PanelHeader(
            ObfuscatedMarkup.CreateSafeSource(header.Text, secretObfuscator),
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

    private Segment[] ObfuscateLinks(Segment[] segments) =>
        [.. segments.Select(segment => segment.IsControlCode
            ? ObfuscateControlCode(segment)
            : segment.Link is null
                ? segment
                : new Segment(segment.Text, segment.Style, ObfuscateLink(segment.Link)))];

    private Segment ObfuscateControlCode(Segment segment) =>
        Segment.Control(ObfuscateMetadata(segment.Text));

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
        var widthChange = width - innerMaximumWidth;
        var minimumWidth = Math.Clamp(innerMinimumWidth + widthChange, 0, width);
        return new Measurement(minimumWidth, width);
    }

    private sealed class SegmentSnapshotRenderable(
        Segment[] segments,
        Measurement innerMeasurement) : IRenderable
    {
        public Measurement Measure(RenderOptions options, int maxWidth) =>
            MeasureSegments(segments, maxWidth, innerMeasurement);

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
