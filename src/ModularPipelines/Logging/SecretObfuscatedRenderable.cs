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
    private readonly IRenderable _inner = PrepareCompositeLayout(inner, secretObfuscator);

    public Measurement Measure(RenderOptions options, int maxWidth)
    {
        var innerMeasurement = ((IRenderable) _inner).Measure(options, maxWidth);
        return MeasureSegments(GetSegments(options, maxWidth), maxWidth, innerMeasurement);
    }

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth) =>
        GetSegments(options, maxWidth);

    internal IRenderable Snapshot(RenderOptions options, int maxWidth)
    {
        var innerMeasurement = ((IRenderable) _inner).Measure(options, maxWidth);
        return new SegmentSnapshotRenderable(GetSegments(options, maxWidth), innerMeasurement);
    }

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

    private static IRenderable PrepareCompositeLayout(
        IRenderable renderable,
        ISecretObfuscator secretObfuscator) => renderable switch
    {
        Align align => PrepareAlign(align, secretObfuscator),
        Columns columns => PrepareColumns(columns, secretObfuscator),
        Grid grid => PrepareGrid(grid, secretObfuscator),
        Padder padder => PreparePadder(padder, secretObfuscator),
        Panel panel => PreparePanel(panel, secretObfuscator),
        Rule rule => PrepareRule(rule, secretObfuscator),
        Rows rows => PrepareRows(rows, secretObfuscator),
        Table table => PrepareTable(table, secretObfuscator),
        Tree tree => PrepareTree(tree, secretObfuscator),
        _ => renderable,
    };

    private static Align PrepareAlign(Align align, ISecretObfuscator secretObfuscator) =>
        new(new SecretObfuscatedRenderable(GetAlignChild(align), secretObfuscator),
            align.Horizontal,
            align.Vertical)
        {
            Height = align.Height,
            Width = align.Width,
        };

    private static Columns PrepareColumns(
        Columns columns,
        ISecretObfuscator secretObfuscator) => new(
        GetColumnItems(columns).Select(
            item => new SecretObfuscatedRenderable(item, secretObfuscator)))
    {
        Expand = columns.Expand,
        Padding = columns.Padding,
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

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_child")]
    private static extern ref readonly IRenderable GetPadderChild(Padder padder);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_child")]
    private static extern ref readonly IRenderable GetPanelChild(Panel panel);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_children")]
    private static extern ref readonly List<IRenderable> GetRowChildren(Rows rows);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_root")]
    private static extern ref readonly TreeNode GetTreeRoot(Tree tree);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_Renderable")]
    private static extern IRenderable GetTreeNodeRenderable(TreeNode node);

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
