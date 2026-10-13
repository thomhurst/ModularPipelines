using System.ComponentModel.DataAnnotations;
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

        if (collectionType == "List<string>?")
        {
            // Mutable contracts retain incompatible typed views rather than replace the source.
            await Assert.That(property.GetValue(instance)).IsSameReferenceAs(input);
        }
        else
        {
            await Assert.That(input.PairEnumerationCount).IsEqualTo(joined ? 0 : 1);
            await Assert.That(input.KeyValueEnumerationCount).IsEqualTo(joined ? 1 : 0);
        }
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
        var baseProperty = inherited ? $"public {collectionType} Values {{ get; set; }}" : "";
        var options = Compile(generated,
            $"namespace ModularPipelines.Tool.Options {{ public partial record ToolOptions {{ {baseProperty} }} }}")
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new SingleUseStringRendererViews(entry);
        var property = options.GetProperty("Values")!;
        property.SetValue(instance, input);
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
    [Arguments(" ")]
    [Arguments("valid")]
    public async Task Validated_Global_Overrides_Snapshot_Ordinary_SingleUse_Values(string entry)
    {
        var options = Compile(await GenerateValidatedStringCollection(inherited: true),
            "namespace ModularPipelines.Tool.Options { public partial record ToolOptions { public IEnumerable<string>? Values { get; set; } } }")
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

        options.GetProperty("Values")!.SetValue(instance, Values());
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(string.IsNullOrWhiteSpace(entry));
            await Assert.That(RenderAlternativeCollection(instance, positional: false)).IsEquivalentTo(["--requirement", entry]);
        }

        await Assert.That(enumerationCount).IsEqualTo(1);
        options.GetProperty("Values")!.SetValue(instance, null);
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
        await Assert.That(options.BaseType!.GetProperty("Values")!.GetValue(instance)).IsNull();
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
    public async Task Validated_Mutable_Object_Collections_Preserve_Ordinary_And_Renderer_Views(
        [Matrix("IList<object>?", "ICollection<object>?", "List<object>?", "System.Collections.IList?")] string collectionType,
        [Matrix(false, true)] bool joined,
        [Matrix(false, true)] bool inherited,
        [Matrix(null, "", " ", "valid")] string? entry)
    {
        var separator = joined ? "," : null;
        var generated = await GenerateValidatedStringCollection(collectionType: collectionType,
            collectionSeparator: separator, inherited: inherited);
        var baseProperty = inherited ? $"public {collectionType} Values {{ get; set; }}" : "";
        var options = Compile(generated,
            $"namespace ModularPipelines.Tool.Options {{ public partial record ToolOptions {{ {baseProperty} }} }}")
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new MutableObjectRendererViews(entry);
        var property = options.GetProperty("Values")!;
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
        if (inherited)
        {
            await Assert.That(options.BaseType!.GetProperty("Values")!.GetValue(instance)).IsSameReferenceAs(retained);
        }

        await Assert.That(input.OrdinaryEnumerationCount).IsEqualTo(1);
        await Assert.That(input.PairEnumerationCount).IsEqualTo(joined ? 0 : 1);
        await Assert.That(input.KeyValueEnumerationCount).IsEqualTo(joined ? 1 : 0);
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

    private static Task<string> GenerateValidatedStringCollection(
        bool alternative = false, bool rejectBlank = true, string collectionType = "IEnumerable<string>?", string? collectionSeparator = null, bool inherited = false) =>
        Generate([new CliOptionDefinition
        {
            SwitchName = "--requirement",
            PropertyName = "Values",
            CSharpType = collectionType,
            AcceptsMultipleValues = true,
            RejectBlankCollectionValues = rejectBlank,
            CollectionSeparator = collectionSeparator,
        }], alternativeGroups: alternative
            ? [new() { Members = [new() { PropertyName = "Values", OptionSwitch = "--requirement" }] }]
            : [], globalOptions: inherited
            ? [new() { SwitchName = "--requirement", PropertyName = "Values", CSharpType = collectionType, AcceptsMultipleValues = true }]
            : null);
}
