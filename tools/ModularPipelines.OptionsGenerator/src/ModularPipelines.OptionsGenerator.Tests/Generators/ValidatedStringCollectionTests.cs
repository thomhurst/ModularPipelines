using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [MatrixDataSource]
    public async Task Validated_String_Collections_Reject_Blank_Entries(
        [Matrix(null, "", " ", "\t", "development")] string? entry,
        [Matrix(false, true)] bool mixed,
        [Matrix(false, true)] bool alternative)
    {
        var options = Compile(await GenerateValidatedStringCollection(alternative))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var enumerationCount = 0;
        IEnumerable<string> Values()
        {
            if (++enumerationCount > 1)
            {
                throw new InvalidOperationException("Input was enumerated twice.");
            }

            if (mixed)
            {
                yield return "testing";
            }

            yield return entry!;
        }

        options.GetProperty("Values")!.SetValue(instance, Values());
        for (var attempt = 0; attempt < 2; attempt++)
        {
            var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
            await Assert.That(errors.Length == 0).IsEqualTo(!string.IsNullOrWhiteSpace(entry));
            if (string.IsNullOrWhiteSpace(entry))
            {
                var blankError = errors.Single(error => error.ErrorMessage == "Values cannot contain null, empty, or whitespace values.");
                await Assert.That(blankError.MemberNames).IsEquivalentTo(["Values"]);
            }
            else
            {
                string[] expected = mixed
                    ? ["--requirement", "testing", "--requirement", entry!]
                    : ["--requirement", entry!];
                await Assert.That(RenderAlternativeCollection(instance, positional: false)).IsEquivalentTo(expected);
            }
        }

        await Assert.That(enumerationCount).IsEqualTo(1);
    }

    [Test]
    public async Task Validated_Optional_String_Collections_Allow_Null_And_Empty_Collections()
    {
        var options = Compile(await GenerateValidatedStringCollection())
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        foreach (var values in new IEnumerable<string>?[] { null, [] })
        {
            options.GetProperty("Values")!.SetValue(instance, values);
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            await Assert.That(RenderAlternativeCollection(instance, positional: false)).IsEmpty();
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task String_Collection_Blank_Validation_Is_Opt_In(bool alternative)
    {
        var options = Compile(await GenerateValidatedStringCollection(alternative, rejectBlank: false))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        string[] values = [null!, "", " ", "valid"];
        var property = options.GetProperty("Values")!;
        property.SetValue(instance, values);
        if (alternative)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
        }
        else
        {
            await Assert.That(instance is IValidatableObject).IsFalse();
            await Assert.That(property.GetValue(instance)).IsSameReferenceAs(values);
        }
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_String_Collections_Preserve_Renderer_Views(
        [Matrix("IEnumerable<string>?", "IReadOnlyList<string>?", "List<string>?")] string collectionType,
        [Matrix(false, true)] bool joined)
    {
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(collectionType: collectionType, collectionSeparator: separator))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new StringRendererViews(singleUse: collectionType != "List<string>?");
        var property = options.GetProperty("Values")!;
        property.SetValue(instance, input);
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            string[] expected = joined ? ["--requirement", "typed=value"] : ["--requirement", "typed", "value"];
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(expected);
        }

        await Assert.That(input.PairEnumerationCount).IsEqualTo(joined ? 0 : 1);
        await Assert.That(input.KeyValueEnumerationCount).IsEqualTo(joined ? 1 : 0);
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_Multiple_Views_Snapshot_SingleUse_String_Entries(
        [Matrix("IEnumerable<string>?", "IReadOnlyCollection<string>?", "IReadOnlyList<string>?", "IReadOnlySet<string>?",
            "System.Collections.IEnumerable?", "IEnumerable<object>?", "IReadOnlyCollection<object>?", "IReadOnlyList<object>?")] string collectionType,
        [Matrix(false, true)] bool joined,
        [Matrix(false, true)] bool inherited,
        [Matrix(" ", "valid")] string entry)
    {
        var separator = joined ? "," : null;
        var generated = await GenerateValidatedStringCollection(collectionType: collectionType,
            collectionSeparator: separator, inherited: inherited);
        var options = Compile(generated)
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new SingleUseStringRendererViews(entry);
        var property = options.GetProperty("Values")!;
        property.SetValue(instance, collectionType == "IReadOnlySet<string>?" ? new ComparerAwareRendererSet(entry, input) : input);
        var retained = property.GetValue(instance);
        for (var pass = 0; pass < 2; pass++)
        {
            var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
            await Assert.That(errors.Length != 0).IsEqualTo(string.IsNullOrWhiteSpace(entry));
            await Assert.That(System.Linq.Enumerable.Cast<string>((System.Collections.IEnumerable) retained!)).IsEquivalentTo(["first", entry]);
            string[] expected = joined ? ["--requirement", "typed=value"] : ["--requirement", "typed", "value"];
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(expected);
        }

        if (retained is IReadOnlyCollection<string> collection)
        {
            await Assert.That(collection.Count).IsEqualTo(2);
        }

        if (retained is IReadOnlyList<string> list)
        {
            await Assert.That(list[1]).IsEqualTo(entry);
        }

        if (retained is IReadOnlySet<string> set)
        {
            await Assert.That(set.Contains("FIRST")).IsTrue();
            await Assert.That(set.SetEquals(["FIRST", entry.ToUpperInvariant()])).IsTrue();
        }

        if (inherited)
        {
            await Assert.That(options.BaseType!.GetProperty("Values")!.GetValue(instance)).IsSameReferenceAs(retained);
        }

        await Assert.That(input.StringEnumerationCount).IsEqualTo(1);
        await Assert.That(input.PairEnumerationCount).IsEqualTo(joined ? 0 : 1);
        await Assert.That(input.KeyValueEnumerationCount).IsEqualTo(joined ? 1 : 0);
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_Global_Overrides_Snapshot_Ordinary_SingleUse_Values(
        [Matrix(" ", "valid")] string entry,
        [Matrix(false, true)] bool baseAssignment)
    {
        var options = Compile(await GenerateValidatedStringCollection(inherited: true))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var enumerationCount = 0;
        IEnumerable<string> Values()
        {
            if (++enumerationCount > 1)
            {
                throw new InvalidOperationException("Input was enumerated twice.");
            }

            yield return entry;
        }

        var property = (baseAssignment ? options.BaseType! : options).GetProperty("Values")!;
        property.SetValue(instance, Values());
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(string.IsNullOrWhiteSpace(entry));
            await Assert.That(RenderAlternativeCollection(instance, positional: false)).IsEquivalentTo(["--requirement", entry]);
        }

        await Assert.That(enumerationCount).IsEqualTo(1);
        property.SetValue(instance, null);
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
        await Assert.That(options.BaseType!.GetProperty("Values")!.GetValue(instance)).IsNull();
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_Base_Assignments_Snapshot_All_Renderer_Views(
        [Matrix("IEnumerable<string>?", "IReadOnlyCollection<string>?", "IReadOnlyList<string>?", "IReadOnlySet<string>?",
            "System.Collections.IEnumerable?", "IEnumerable<object>?", "IReadOnlyCollection<object>?", "IReadOnlyList<object>?")] string collectionType,
        [Matrix(false, true)] bool joined,
        [Matrix(" ", "valid")] string entry)
    {
        var separator = joined ? "," : null;
        var generated = await GenerateValidatedStringCollection(collectionType: collectionType,
            collectionSeparator: separator, inherited: true);
        var options = Compile(generated)
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new SingleUseStringRendererViews(entry);
        var baseProperty = options.BaseType!.GetProperty("Values")!;
        var property = options.GetProperty("Values")!;
        baseProperty.SetValue(instance, collectionType == "IReadOnlySet<string>?" ? new ComparerAwareRendererSet(entry, input) : input);
        var retained = property.GetValue(instance);
        await Assert.That(baseProperty.GetValue(instance)).IsSameReferenceAs(retained);
        var attribute = Attribute.GetCustomAttributes(property, inherit: true)
            .Single(attribute => attribute.GetType().Name == "CliOptionAttribute");
        await Assert.That(attribute.GetType().GetProperty("ShortForm")!.GetValue(attribute)).IsNull();
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(string.IsNullOrWhiteSpace(entry));
            await Assert.That(((System.Collections.IEnumerable) retained!).Cast<string>()).IsEquivalentTo(["first", entry]);
            string[] expected = joined ? ["--requirement", "typed=value"] : ["--requirement", "typed", "value"];
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(expected);
        }

        await Assert.That(input.StringEnumerationCount).IsEqualTo(1);
        await Assert.That(input.PairEnumerationCount).IsEqualTo(joined ? 0 : 1);
        await Assert.That(input.KeyValueEnumerationCount).IsEqualTo(joined ? 1 : 0);
        baseProperty.SetValue(instance, null);
        await Assert.That(property.GetValue(instance)).IsNull();
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_Required_Alternate_Input_Base_Assignments_Snapshot_Ordinary_Values(
        [Matrix(" ", "valid")] string entry,
        [Matrix(false, true)] bool joined,
        [Matrix(false, true)] bool factory)
    {
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(collectionSeparator: separator,
                inherited: true, required: true))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        string[] initial = ["initial"];
        var instance = factory
            ? options.GetMethod("FromCliInputJson")!.Invoke(null, ["{}"])!
            : Activator.CreateInstance(options, [initial])!;
        var enumerationCount = 0;
        IEnumerable<string> Values()
        {
            if (++enumerationCount > 1)
            {
                throw new InvalidOperationException("Input was enumerated twice.");
            }

            yield return "first";
            yield return entry;
        }

        var property = options.BaseType!.GetProperty("Values")!;
        property.SetValue(instance, Values());
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(string.IsNullOrWhiteSpace(entry));
            string[] expected = joined ? ["--requirement", $"first,{entry}"] : ["--requirement", "first", "--requirement", entry];
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(expected);
        }

        await Assert.That(enumerationCount).IsEqualTo(1);
        property.SetValue(instance, null);
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_Required_Alternate_Input_Base_Assignments_Snapshot_Values(
        [Matrix("IEnumerable<string>?", "IReadOnlyCollection<string>?", "IReadOnlyList<string>?", "IReadOnlySet<string>?",
            "System.Collections.IEnumerable?", "IEnumerable<object>?", "IReadOnlyCollection<object>?", "IReadOnlyList<object>?")] string collectionType,
        [Matrix(false, true)] bool joined,
        [Matrix(" ", "valid")] string entry,
        [Matrix(false, true)] bool factory)
    {
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(collectionType: collectionType,
                collectionSeparator: separator, inherited: true, required: true))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        object initial = collectionType == "IReadOnlySet<string>?" ? new HashSet<string> { "initial" } : new[] { "initial" };
        var instance = factory
            ? options.GetMethod("FromCliInputJson")!.Invoke(null, ["{}"])!
            : Activator.CreateInstance(options, [initial])!;
        var baseProperty = options.BaseType!.GetProperty("Values")!;
        var property = options.GetProperty("Values")!;
        await Assert.That(property.SetMethod!.IsPrivate).IsTrue();
        await Assert.That(new System.Reflection.NullabilityInfoContext().Create(property).ReadState)
            .IsEqualTo(System.Reflection.NullabilityState.Nullable);
        var input = new SingleUseStringRendererViews(entry);
        baseProperty.SetValue(instance, collectionType == "IReadOnlySet<string>?" ? new ComparerAwareRendererSet(entry, input) : input);
        var retained = property.GetValue(instance);
        await Assert.That(baseProperty.GetValue(instance)).IsSameReferenceAs(retained);
        for (var pass = 0; pass < 2; pass++)
        {
            var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
            await Assert.That(errors.Length).IsEqualTo(string.IsNullOrWhiteSpace(entry) ? 1 : 0);
            if (errors.Length > 0)
            {
                await Assert.That(errors[0].MemberNames).IsEquivalentTo(["Values"]);
            }

            await Assert.That(((System.Collections.IEnumerable) retained!).Cast<string>()).IsEquivalentTo(["first", entry]);
            string[] expected = joined ? ["--requirement", "typed=value"] : ["--requirement", "typed", "value"];
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(expected);
        }

        await Assert.That(input.StringEnumerationCount).IsEqualTo(1);
        await Assert.That(input.PairEnumerationCount).IsEqualTo(joined ? 0 : 1);
        await Assert.That(input.KeyValueEnumerationCount).IsEqualTo(joined ? 1 : 0);
        baseProperty.SetValue(instance, null);
        await Assert.That(property.GetValue(instance)).IsNull();
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_ReadOnly_Sets_Snapshot_Ordinary_SingleUse_Values(
        [Matrix("local", "command", "base")] string assignment,
        [Matrix(false, true)] bool joined,
        [Matrix(" ", "valid")] string entry)
    {
        var inherited = assignment != "local";
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(collectionType: "IReadOnlySet<string>?",
                collectionSeparator: separator, inherited: inherited))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new SingleUseReadOnlySet(entry);
        var property = (assignment == "base" ? options.BaseType! : options).GetProperty("Values")!;
        property.SetValue(instance, input);
        var retained = (IReadOnlySet<string>) property.GetValue(instance)!;
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(string.IsNullOrWhiteSpace(entry));
            await Assert.That(retained).IsEquivalentTo(["first", entry]);
            string[] expected = joined ? ["--requirement", $"first,{entry}"] : ["--requirement", "first", "--requirement", entry];
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(expected);
        }

        await Assert.That(retained.Count).IsEqualTo(2);
        await Assert.That(retained.Contains("FIRST")).IsTrue();
        await Assert.That(retained.SetEquals(["FIRST", entry.ToUpperInvariant()])).IsTrue();
        await Assert.That(input.EnumerationCount).IsEqualTo(1);
        property.SetValue(instance, null);
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_ReadOnly_Set_Operations_Use_Retained_Values(
        [Matrix(false, true)] bool sorted,
        [Matrix("local", "command", "base")] string assignment,
        [Matrix(false, true)] bool joined,
        [Matrix(false, true)] bool ignoreCase)
    {
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(collectionType: "IReadOnlySet<string>?",
                collectionSeparator: separator, inherited: assignment != "local"))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        ISet<string> input = sorted
            ? new SortedSet<string>(["first", "valid"], comparer)
            : new HashSet<string>(["first", "valid"], comparer);
        var property = (assignment == "base" ? options.BaseType! : options).GetProperty("Values")!;
        property.SetValue(instance, input);
        var retained = (IReadOnlySet<string>) property.GetValue(instance)!;
        input.Clear();
        input.Add(" ");

        await Assert.That(retained).IsEquivalentTo(["first", "valid"]);
        await Assert.That(retained.Count).IsEqualTo(2);
        await Assert.That(retained.Contains("FIRST")).IsEqualTo(ignoreCase);
        await Assert.That(retained.Contains(" ")).IsFalse();
        await Assert.That(retained.IsProperSubsetOf(["FIRST", "VALID", "extra"])).IsEqualTo(ignoreCase);
        await Assert.That(retained.IsProperSupersetOf(["FIRST"])).IsEqualTo(ignoreCase);
        await Assert.That(retained.IsSubsetOf(["FIRST", "VALID"])).IsEqualTo(ignoreCase);
        await Assert.That(retained.IsSupersetOf(["FIRST", "VALID"])).IsEqualTo(ignoreCase);
        await Assert.That(retained.Overlaps(["VALID"])).IsEqualTo(ignoreCase);
        await Assert.That(retained.SetEquals(["FIRST", "VALID", "first"])).IsEqualTo(ignoreCase);
        await Assert.That(retained.SetEquals(["first", "valid", "first"])).IsTrue();
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
        string[] expected = joined ? ["--requirement", "first,valid"] : ["--requirement", "first", "--requirement", "valid"];
        await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
            .IsEquivalentTo(expected);
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_ReadOnly_Set_Renderer_Adapters_Use_Retained_Values(
        [Matrix(false, true)] bool joined,
        [Matrix(false, true)] bool baseAssignment)
    {
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(collectionType: "IReadOnlySet<string>?",
                collectionSeparator: separator, inherited: true))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new MutableRendererSet();
        var property = (baseAssignment ? options.BaseType! : options).GetProperty("Values")!;
        property.SetValue(instance, input);
        var retained = (IReadOnlySet<string>) property.GetValue(instance)!;
        input.Clear();
        input.Add(" ");
        input.RendererValue = "changed";

        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(retained).IsEquivalentTo(["first", "valid"]);
            await Assert.That(retained.Count).IsEqualTo(2);
            await Assert.That(retained.Contains("FIRST")).IsTrue();
            await Assert.That(retained.Contains(" ")).IsFalse();
            await Assert.That(retained.IsProperSubsetOf(["FIRST", "VALID", "extra"])).IsTrue();
            await Assert.That(retained.IsProperSupersetOf(["FIRST"])).IsTrue();
            await Assert.That(retained.IsSubsetOf(["FIRST", "VALID"])).IsTrue();
            await Assert.That(retained.IsSupersetOf(["FIRST", "VALID"])).IsTrue();
            await Assert.That(retained.Overlaps(["VALID"])).IsTrue();
            await Assert.That(retained.SetEquals(["FIRST", "VALID", "first"])).IsTrue();
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            string[] expected = joined ? ["--requirement", "typed=value"] : ["--requirement", "typed", "value"];
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(expected);
        }

        await Assert.That(input.PairEnumerationCount).IsEqualTo(joined ? 0 : 1);
        await Assert.That(input.KeyValueEnumerationCount).IsEqualTo(joined ? 1 : 0);
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_Opaque_ReadOnly_Sets_Retain_Consistent_Source_Semantics(
        [Matrix(false, true)] bool joined,
        [Matrix(false, true)] bool rendererView)
    {
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(collectionType: "IReadOnlySet<string>?",
                collectionSeparator: separator))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var members = new HashSet<string>(["first", "valid"], StringComparer.OrdinalIgnoreCase);
        var source = new OpaqueReadOnlySet(members);
        object input = rendererView ? new OpaqueRendererSet(members) : source;
        var property = options.GetProperty("Values")!;
        property.SetValue(instance, input);
        var retained = (IReadOnlySet<string>) property.GetValue(instance)!;
        members.Clear();
        members.Add(" ");

        await Assert.That(retained).IsEquivalentTo([" "]);
        await Assert.That(retained.Count).IsEqualTo(1);
        await Assert.That(retained.Contains("FIRST")).IsFalse();
        await Assert.That(retained.Contains(" ")).IsTrue();
        await Assert.That(retained.SetEquals([" "])).IsTrue();
        await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any()).IsTrue();
        string[] expected = ["--requirement", " "];
        if (rendererView)
        {
            expected = joined ? ["--requirement", "typed=value"] : ["--requirement", "typed", "value"];
        }
        await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
            .IsEquivalentTo(expected);
    }

    private class OpaqueReadOnlySet(HashSet<string> members) : IReadOnlySet<string>
    {
        public int Count => members.Count;
        public bool Contains(string item) => members.Contains(item);
        public bool IsProperSubsetOf(IEnumerable<string> other) => members.IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<string> other) => members.IsProperSupersetOf(other);
        public bool IsSubsetOf(IEnumerable<string> other) => members.IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<string> other) => members.IsSupersetOf(other);
        public bool Overlaps(IEnumerable<string> other) => members.Overlaps(other);
        public bool SetEquals(IEnumerable<string> other) => members.SetEquals(other);
        public IEnumerator<string> GetEnumerator() => members.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class OpaqueRendererSet(HashSet<string> members) : OpaqueReadOnlySet(members),
        IEnumerable<ModularPipelines.Models.CliValuePair>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator() =>
            ((IEnumerable<ModularPipelines.Models.CliValuePair>) [new ModularPipelines.Models.CliValuePair("typed", "value")]).GetEnumerator();

        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator() =>
            ((IEnumerable<ModularPipelines.Models.KeyValue>) [new ModularPipelines.Models.KeyValue("typed", "value")]).GetEnumerator();
    }

    private sealed class MutableRendererSet() : HashSet<string>(["first", "valid"], StringComparer.OrdinalIgnoreCase),
        IEnumerable<ModularPipelines.Models.CliValuePair>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        public string RendererValue { get; set; } = "value";
        public int PairEnumerationCount { get; private set; }
        public int KeyValueEnumerationCount { get; private set; }

        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator()
        {
            PairEnumerationCount++;
            yield return new("typed", RendererValue);
        }

        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator()
        {
            KeyValueEnumerationCount++;
            yield return new("typed", RendererValue);
        }
    }

    private sealed class SingleUseReadOnlySet(string entry) : HashSet<string>(["first", entry], StringComparer.OrdinalIgnoreCase), IReadOnlySet<string>
    {
        public int EnumerationCount { get; private set; }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            if (++EnumerationCount > 1)
            {
                throw new InvalidOperationException("Set input can only be enumerated once.");
            }

            return base.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((IEnumerable<string>) this).GetEnumerator();
    }

    private sealed class ComparerAwareRendererSet(string entry, SingleUseStringRendererViews values)
        : HashSet<string>(["first", entry], StringComparer.OrdinalIgnoreCase), IReadOnlySet<string>,
            IEnumerable<ModularPipelines.Models.CliValuePair>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        IEnumerator<string> IEnumerable<string>.GetEnumerator() => values.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => values.GetEnumerator();
        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator() =>
            ((IEnumerable<ModularPipelines.Models.CliValuePair>) values).GetEnumerator();
        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator() =>
            ((IEnumerable<ModularPipelines.Models.KeyValue>) values).GetEnumerator();
    }

    private sealed class SingleUseStringRendererViews(string entry) : IReadOnlyList<string>, IReadOnlySet<string>,
        IEnumerable<ModularPipelines.Models.CliValuePair>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        private readonly HashSet<string> _members = new(["first", entry], StringComparer.OrdinalIgnoreCase);

        public int StringEnumerationCount { get; private set; }
        public int PairEnumerationCount { get; private set; }
        public int KeyValueEnumerationCount { get; private set; }
        public int Count => 2;
        public string this[int index] => index == 0 ? "first" : entry;
        public bool Contains(string item) => _members.Contains(item);
        public bool IsProperSubsetOf(IEnumerable<string> other) => _members.IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<string> other) => _members.IsProperSupersetOf(other);
        public bool IsSubsetOf(IEnumerable<string> other) => _members.IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<string> other) => _members.IsSupersetOf(other);
        public bool Overlaps(IEnumerable<string> other) => _members.Overlaps(other);
        public bool SetEquals(IEnumerable<string> other) => _members.SetEquals(other);

        public IEnumerator<string> GetEnumerator()
        {
            if (++StringEnumerationCount > 1)
            {
                throw new InvalidOperationException("String input can only be enumerated once.");
            }

            yield return "first";
            yield return entry;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator()
        {
            if (++PairEnumerationCount > 1)
            {
                throw new InvalidOperationException("Pair input can only be enumerated once.");
            }

            yield return new("typed", "value");
        }

        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator()
        {
            if (++KeyValueEnumerationCount > 1)
            {
                throw new InvalidOperationException("KeyValue input can only be enumerated once.");
            }

            yield return new("typed", "value");
        }
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_Mutable_Collections_Preserve_Ordinary_And_Renderer_Views(
        [Matrix("IList<object>?", "ICollection<object>?", "List<object>?", "System.Collections.IList?",
            "IList<string>?", "ICollection<string>?", "List<string>?")] string collectionType,
        [Matrix(false, true)] bool joined,
        [Matrix(0, 1, 2)] int assignment,
        [Matrix(null, "", " ", "valid")] string? entry)
    {
        var separator = joined ? "," : null;
        var generated = await GenerateValidatedStringCollection(collectionType: collectionType,
            collectionSeparator: separator, inherited: assignment != 0);
        var options = Compile(generated)
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        object input = collectionType.Contains("string", StringComparison.Ordinal)
            ? new MutableStringRendererViews(entry)
            : new MutableObjectRendererViews(entry);
        var property = (assignment == 2 ? options.BaseType! : options).GetProperty("Values")!;
        property.SetValue(instance, input);
        var retained = (System.Collections.IList) property.GetValue(instance)!;
        for (var pass = 0; pass < 2; pass++)
        {
            var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
            await Assert.That(errors.Length != 0).IsEqualTo(string.IsNullOrWhiteSpace(entry));
            await Assert.That(retained.Count).IsEqualTo(2);
            await Assert.That(retained[1]).IsEqualTo(entry);
            await Assert.That(System.Linq.Enumerable.Cast<string>(retained)).IsEquivalentTo(["first", entry]);
            string[] expected = joined ? ["--requirement", "typed=value"] : ["--requirement", "typed", "value"];
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(expected);
        }

        retained[1] = "updated";
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
        string[] renderedAfterMutation = joined ? ["--requirement", "typed=value"] : ["--requirement", "typed", "value"];
        await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
            .IsEquivalentTo(renderedAfterMutation);
        if (assignment != 0)
        {
            await Assert.That(options.BaseType!.GetProperty("Values")!.GetValue(instance)).IsSameReferenceAs(retained);
        }

        var (ordinaryEnumerationCount, pairEnumerationCount, keyValueEnumerationCount) = input is MutableStringRendererViews strings
            ? (strings.OrdinaryEnumerationCount, strings.PairEnumerationCount, strings.KeyValueEnumerationCount)
            : (((MutableObjectRendererViews) input).OrdinaryEnumerationCount,
                ((MutableObjectRendererViews) input).PairEnumerationCount, ((MutableObjectRendererViews) input).KeyValueEnumerationCount);
        await Assert.That(ordinaryEnumerationCount).IsEqualTo(1);
        await Assert.That(pairEnumerationCount).IsEqualTo(joined ? 0 : 1);
        await Assert.That(keyValueEnumerationCount).IsEqualTo(joined ? 1 : 0);
    }

    private sealed class MutableStringRendererViews(string? entry) : List<string>(["first", entry!]),
        IEnumerable<string>, IEnumerable<ModularPipelines.Models.CliValuePair>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        public int OrdinaryEnumerationCount { get; private set; }
        public int PairEnumerationCount { get; private set; }
        public int KeyValueEnumerationCount { get; private set; }

        IEnumerator<string> IEnumerable<string>.GetEnumerator() => OrdinaryValues();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => OrdinaryValues();

        private List<string>.Enumerator OrdinaryValues()
        {
            if (++OrdinaryEnumerationCount > 1)
            {
                throw new InvalidOperationException("Ordinary input can only be enumerated once.");
            }

            return base.GetEnumerator();
        }

        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator()
        {
            if (++PairEnumerationCount > 1)
            {
                throw new InvalidOperationException("Pair input can only be enumerated once.");
            }

            yield return new("typed", "value");
        }

        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator()
        {
            if (++KeyValueEnumerationCount > 1)
            {
                throw new InvalidOperationException("KeyValue input can only be enumerated once.");
            }

            yield return new("typed", "value");
        }
    }

    [Test]
    [MatrixDataSource]
    public async Task Mutable_String_Renderer_Snapshot_Is_Opt_In(
        [Matrix("List<string>?", "IList<string>?", "ICollection<string>?")] string collectionType,
        [Matrix(false, true)] bool joined,
        [Matrix(0, 1, 2)] int assignment)
    {
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(rejectBlank: false,
                collectionType: collectionType, collectionSeparator: separator, inherited: assignment != 0))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new MutableStringRendererViews("valid");
        var property = (assignment == 2 ? options.BaseType! : options).GetProperty("Values")!;
        property.SetValue(instance, input);
        await Assert.That(property.GetValue(instance)).IsSameReferenceAs(input);
        await Assert.That(input.OrdinaryEnumerationCount).IsEqualTo(0);
        string[] expected = joined ? ["--requirement", "typed=value"] : ["--requirement", "typed", "value"];
        await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
            .IsEquivalentTo(expected);
    }

    [Test]
    [MatrixDataSource]
    public async Task Required_Mutable_String_Renderer_Snapshots_Intercept_Base_Assignments(
        [Matrix("List<string>?", "IList<string>?", "ICollection<string>?")] string collectionType,
        [Matrix(false, true)] bool joined,
        [Matrix(false, true)] bool factory,
        [Matrix(null, "valid")] string? entry)
    {
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(collectionType: collectionType,
                collectionSeparator: separator, inherited: true, required: true))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = factory
            ? options.GetMethod("FromCliInputJson")!.Invoke(null, ["{}"])!
            : Activator.CreateInstance(options, [new List<string> { "initial" }])!;
        var input = new MutableStringRendererViews(entry);
        options.BaseType!.GetProperty("Values")!.SetValue(instance, input);
        var retained = (IList<string>) options.GetProperty("Values")!.GetValue(instance)!;
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(string.IsNullOrWhiteSpace(entry));
            await Assert.That(retained).IsEquivalentTo(["first", entry]);
            string[] expected = joined ? ["--requirement", "typed=value"] : ["--requirement", "typed", "value"];
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(expected);
        }

        await Assert.That(input.OrdinaryEnumerationCount).IsEqualTo(1);
        await Assert.That(input.PairEnumerationCount).IsEqualTo(joined ? 0 : 1);
        await Assert.That(input.KeyValueEnumerationCount).IsEqualTo(joined ? 1 : 0);
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_Object_Collections_Report_Non_String_Entries(
        [Matrix("IEnumerable<object>?", "IReadOnlyCollection<object>?", "IReadOnlyList<object>?",
            "List<object>?", "IList<object>?", "ICollection<object>?", "System.Collections.IEnumerable?",
            "System.Collections.IList?")] string collectionType,
        [Matrix(0, 1, 2)] int assignment)
    {
        var options = Compile(await GenerateValidatedStringCollection(collectionType: collectionType,
                inherited: assignment != 0)).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = (assignment == 2 ? options.BaseType! : options).GetProperty("Values")!;
        property.SetValue(instance, new List<object> { "first", 42 });
        for (var pass = 0; pass < 2; pass++)
        {
            var error = ((IValidatableObject) instance).Validate(new(instance)).Single();
            await Assert.That(error.ErrorMessage).IsEqualTo("Values cannot contain non-string values.");
            await Assert.That(error.MemberNames).IsEquivalentTo(["Values"]);
        }
    }

    private sealed class MutableObjectRendererViews(string? entry) : List<object>(["first", entry!]),
        IEnumerable<ModularPipelines.Models.CliValuePair>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        public int OrdinaryEnumerationCount { get; private set; }
        public int PairEnumerationCount { get; private set; }
        public int KeyValueEnumerationCount { get; private set; }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            if (++OrdinaryEnumerationCount > 1)
            {
                throw new InvalidOperationException("Ordinary input can only be enumerated once.");
            }

            return base.GetEnumerator();
        }

        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator()
        {
            if (++PairEnumerationCount > 1)
            {
                throw new InvalidOperationException("Pair input can only be enumerated once.");
            }

            yield return new("typed", "value");
        }

        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator()
        {
            if (++KeyValueEnumerationCount > 1)
            {
                throw new InvalidOperationException("KeyValue input can only be enumerated once.");
            }

            yield return new("typed", "value");
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Validated_Collections_Skip_Case_Only_Duplicate_Properties(bool validateFirst)
    {
        var options = Compile(await Generate([
            new() { SwitchName = "--first", PropertyName = "Values", CSharpType = "IEnumerable<string>?", RejectBlankCollectionValues = validateFirst },
            new() { SwitchName = "--second", PropertyName = "values", CSharpType = "IEnumerable<string>?", RejectBlankCollectionValues = true },
        ])).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        string[] values = [" "];
        options.GetProperty("Values")!.SetValue(instance, values);
        var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
        await Assert.That(errors.Length).IsEqualTo(validateFirst ? 1 : 0);
        await Assert.That(options.GetProperty("values")).IsNull();
    }

    [Test]
    [Arguments("Values", "values")]
    [Arguments("values", "Values")]
    public async Task Validated_Collections_Skip_Properties_Owned_By_Primary_Constructor(string optionName, string positionalName)
    {
        var options = Compile(await Generate([
            new() { SwitchName = "--values", PropertyName = optionName, CSharpType = "IEnumerable<string>?", RejectBlankCollectionValues = true },
            new() { SwitchName = "--tags", PropertyName = "Tags", CSharpType = "IEnumerable<string>?", RejectBlankCollectionValues = true },
        ], positionalArguments: [new() { PropertyName = positionalName, CSharpType = "int", IsRequired = true }]))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options, [42])!;
        await Assert.That(options.GetProperty(optionName)).IsNull();
        await Assert.That(options.GetProperty(positionalName)!.GetValue(instance)).IsEqualTo(42);
        string[] tags = [" "];
        options.GetProperty("Tags")!.SetValue(instance, tags);
        var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
        await Assert.That(errors.Length).IsEqualTo(1);
        await Assert.That(errors[0].MemberNames).IsEquivalentTo(["Tags"]);
    }

    [Test]
    public async Task Validated_Required_Collections_Retain_Blank_Entry_Validation()
    {
        var options = Compile(await Generate([
            new() { SwitchName = "--values", PropertyName = "Values", CSharpType = "IEnumerable<string>?", IsRequired = true, RejectBlankCollectionValues = true },
            new() { SwitchName = "--duplicate", PropertyName = "values", CSharpType = "int?" },
        ])).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        string[] values = ["valid", " "];
        var instance = Activator.CreateInstance(options, [values])!;
        var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
        await Assert.That(errors.Length).IsEqualTo(1);
        await Assert.That(errors[0].MemberNames).IsEquivalentTo(["Values"]);
        await Assert.That(options.GetProperty("values")).IsNull();
    }

    [Test]
    [Arguments("string[]?")]
    [Arguments("List<string>?")]
    [Arguments("IReadOnlyList<string?>?")]
    [Arguments("System.Collections.Immutable.ImmutableArray<string>?")]
    [Arguments("System.Collections.IEnumerable?")]
    public async Task Validated_String_Collections_Support_Declared_Collection_Contracts(string collectionType)
    {
        var options = Compile(await GenerateValidatedStringCollection(collectionType: collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        foreach (var entry in new string?[] { null, "", " ", "valid" })
        {
            string[] values = ["first", entry!];
            object input = collectionType switch
            {
                "List<string>?" => values.ToList(),
                "System.Collections.Immutable.ImmutableArray<string>?" => System.Collections.Immutable.ImmutableArray.CreateRange(values),
                _ => values,
            };
            options.GetProperty("Values")!.SetValue(instance, input);
            await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(string.IsNullOrWhiteSpace(entry));
        }
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_Character_Views_Snapshot_Ordinary_Values(
        [Matrix("IEnumerable<string>?", "IReadOnlyCollection<string>?", "IReadOnlyList<string>?", "IReadOnlySet<string>?",
            "System.Collections.IEnumerable?", "IEnumerable<object>?", "IReadOnlyCollection<object>?", "IReadOnlyList<object>?",
            "List<string>?", "IList<string>?", "ICollection<string>?", "List<object>?", "IList<object>?",
            "ICollection<object>?", "System.Collections.IList?")] string collectionType,
        [Matrix(null, "", " ", "valid")] string? entry,
        [Matrix(false, true)] bool joined,
        [Matrix(0, 1, 2)] int assignment)
    {
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(collectionType: collectionType,
                collectionSeparator: separator, inherited: assignment != 0))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var views = new CharacterViews(entry);
        object input = collectionType switch
        {
            "IReadOnlySet<string>?" => new CharacterSet(entry, views),
            "List<object>?" or "IList<object>?" or "ICollection<object>?" or "System.Collections.IList?" =>
                new CharacterObjectList(entry, views),
            "List<string>?" or "IList<string>?" or "ICollection<string>?" => new CharacterStringList(entry, views),
            _ => new CharacterReadOnlyList(entry, views),
        };
        var property = (assignment == 2 ? options.BaseType! : options).GetProperty("Values")!;
        property.SetValue(instance, input);
        var retained = property.GetValue(instance)!;
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(string.IsNullOrWhiteSpace(entry));
            await Assert.That(((System.Collections.IEnumerable) retained).Cast<string>()).IsEquivalentTo(["first", entry]);
            await Assert.That(((IEnumerable<char>) retained).ToArray()).IsEquivalentTo("scalar".ToCharArray());
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(["--requirement", "scalar"]);
        }

        await Assert.That(views.StringEnumerationCount).IsEqualTo(1);
        await Assert.That(views.CharacterEnumerationCount).IsEqualTo(1);
        if (retained is IReadOnlySet<string> set)
        {
            await Assert.That(set.Contains("FIRST")).IsTrue();
            await Assert.That(set.SetEquals(["FIRST", entry!])).IsTrue();
        }

        property.SetValue(instance, null);
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
    }

    [Test]
    [MatrixDataSource]
    public async Task Character_Snapshots_Preserve_Renderer_Precedence_And_Opt_In(
        [Matrix(false, true)] bool rejectBlank,
        [Matrix(false, true)] bool joined,
        [Matrix(null, "valid")] string? entry)
    {
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(collectionType: "IReadOnlyList<string>?",
                rejectBlank: rejectBlank, collectionSeparator: separator))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var views = new CharacterViews(entry);
        var input = new CharacterRendererReadOnlyList(entry, views);
        var property = options.GetProperty("Values")!;
        property.SetValue(instance, input);
        for (var pass = 0; pass < 2; pass++)
        {
            if (rejectBlank)
            {
                await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any())
                    .IsEqualTo(string.IsNullOrWhiteSpace(entry));
            }
            else
            {
                await Assert.That(property.GetValue(instance)).IsSameReferenceAs(input);
            }

            string[] expected = joined ? ["--requirement", "scalar"] : ["--requirement", "typed", "value"];
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(expected);
        }

        await Assert.That(views.StringEnumerationCount).IsEqualTo(rejectBlank ? 1 : 0);
        await Assert.That(views.CharacterEnumerationCount).IsEqualTo(rejectBlank && joined ? 1 : 0);
    }

    private static readonly string[] InitialCharacterValues = ["initial"];

    [Test]
    [MatrixDataSource]
    public async Task Required_Character_Snapshots_Intercept_Alternate_Input_Base_Assignments(
        [Matrix(false, true)] bool factory,
        [Matrix(false, true)] bool joined,
        [Matrix(null, "valid")] string? entry)
    {
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(inherited: true, required: true,
                collectionSeparator: separator)).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = factory
            ? options.GetMethod("FromCliInputJson")!.Invoke(null, ["{}"])!
            : Activator.CreateInstance(options, [InitialCharacterValues])!;
        var views = new CharacterViews(entry);
        options.BaseType!.GetProperty("Values")!.SetValue(instance, new CharacterReadOnlyList(entry, views));
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(string.IsNullOrWhiteSpace(entry));
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(["--requirement", "scalar"]);
        }

        await Assert.That(views.StringEnumerationCount).IsEqualTo(1);
        await Assert.That(views.CharacterEnumerationCount).IsEqualTo(1);
    }

    private sealed class CharacterViews(string? entry)
    {
        public int StringEnumerationCount { get; private set; }
        public int CharacterEnumerationCount { get; private set; }

        public IEnumerator<string> Strings()
        {
            if (++StringEnumerationCount > 1)
            {
                throw new InvalidOperationException("String input can only be enumerated once.");
            }

            yield return "first";
            yield return entry!;
        }

        public IEnumerator<char> Characters()
        {
            if (++CharacterEnumerationCount > 1)
            {
                throw new InvalidOperationException("Character input can only be enumerated once.");
            }

            foreach (var character in "scalar")
            {
                yield return character;
            }
        }
    }

    private sealed class CharacterStringList(string? entry, CharacterViews views) : List<string>(["first", entry!]),
        IEnumerable<string>, IEnumerable<char>
    {
        IEnumerator<string> IEnumerable<string>.GetEnumerator() => views.Strings();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => views.Strings();
        IEnumerator<char> IEnumerable<char>.GetEnumerator() => views.Characters();
        public override string ToString() => "scalar";
    }

    private class CharacterReadOnlyList(string? entry, CharacterViews views) : IReadOnlyList<string>, IEnumerable<char>
    {
        public int Count => 2;
        public string this[int index] => index == 0 ? "first" : entry!;
        public IEnumerator<string> GetEnumerator() => views.Strings();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => views.Strings();
        IEnumerator<char> IEnumerable<char>.GetEnumerator() => views.Characters();
        public override string ToString() => "scalar";
    }

    private sealed class CharacterRendererReadOnlyList(string? entry, CharacterViews views) : CharacterReadOnlyList(entry, views),
        IEnumerable<ModularPipelines.Models.CliValuePair>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator() =>
            ((IEnumerable<ModularPipelines.Models.CliValuePair>) [new("typed", "value")]).GetEnumerator();
        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator() =>
            ((IEnumerable<ModularPipelines.Models.KeyValue>) [new("typed", "value")]).GetEnumerator();
    }

    private sealed class CharacterObjectList(string? entry, CharacterViews views) : List<object>(["first", entry!]),
        IEnumerable<object>, IEnumerable<char>
    {
        IEnumerator<object> IEnumerable<object>.GetEnumerator() => views.Strings();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => views.Strings();
        IEnumerator<char> IEnumerable<char>.GetEnumerator() => views.Characters();
        public override string ToString() => "scalar";
    }

    private sealed class CharacterSet(string? entry, CharacterViews views)
        : HashSet<string>(["first", entry!], StringComparer.OrdinalIgnoreCase), IReadOnlySet<string>, IEnumerable<char>
    {
        IEnumerator<string> IEnumerable<string>.GetEnumerator() => views.Strings();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => views.Strings();
        IEnumerator<char> IEnumerable<char>.GetEnumerator() => views.Characters();
        public override string ToString() => "scalar";
    }

    private static async Task<string[]> GenerateValidatedStringCollection(
        bool alternative = false, bool rejectBlank = true, string collectionType = "IEnumerable<string>?", string? collectionSeparator = null, bool inherited = false, bool required = false)
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--requirement",
            PropertyName = "Values",
            CSharpType = collectionType,
            AcceptsMultipleValues = true,
            RejectBlankCollectionValues = rejectBlank,
            IsRequired = required,
            CollectionSeparator = collectionSeparator,
        };
        var tool = new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "src/ModularPipelines.Tool",
            GlobalOptionsBeforeSubcommands = !inherited,
            GlobalOptions = inherited ? [option with { RejectBlankCollectionValues = false, IsRequired = false, ShortForm = "-g" }] : [],
            Commands = [new()
            {
                FullCommand = "tool run", CommandParts = ["run"], ClassName = "ToolRunOptions", ParentClassName = "ToolOptions",
                ToolNamespacePrefix = "Tool", Options = required
                    ? [option, new() { SwitchName = "--cli-input-json", PropertyName = "CliInputJson", CSharpType = "string?" }]
                    : [option],
                RequiredAlternativeGroups = alternative
                    ? [new() { Members = [new() { PropertyName = "Values", OptionSwitch = "--requirement" }] }]
                    : [],
            }],
        };
        var command = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        return inherited
            ? [command, (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content]
            : [command];
    }
}
