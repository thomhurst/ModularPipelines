using System.Collections;
using System.ComponentModel.DataAnnotations;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments(false, "System.Collections.Immutable.ImmutableArray<string>?")]
    [Arguments(true, "System.Collections.Immutable.ImmutableArray<string>?")]
    [Arguments(false, "System.Collections.Immutable.IImmutableList<string>?")]
    [Arguments(true, "System.Collections.Immutable.IImmutableList<string>?")]
    [Arguments(false, "IEnumerable<string>?")]
    [Arguments(true, "IReadOnlyList<string>?")]
    [Arguments(false, "IEnumerable<object>?")]
    [Arguments(true, "IEnumerable<object>?")]
    [Arguments(false, "IReadOnlyList<object>?")]
    [Arguments(true, "IReadOnlyList<object>?")]
    [Arguments(false, "System.Collections.IEnumerable?")]
    [Arguments(true, "System.Collections.IEnumerable?")]
    [Arguments(false, "IList<string>?")]
    [Arguments(true, "IList<string>?")]
    [Arguments(false, "ICollection<string>?")]
    [Arguments(true, "ICollection<string>?")]
    [Arguments(false, "System.Collections.IList?")]
    [Arguments(true, "System.Collections.IList?")]
    public async Task Alternative_Default_ImmutableArrays_Are_Absent_And_Allow_Fallback(bool positional, string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        options.GetProperty("Values")!.SetValue(instance, default(System.Collections.Immutable.ImmutableArray<string>));
        var validation = (IValidatableObject) instance;
        await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        var retained = (IEnumerable) options.GetProperty("Values")!.GetValue(instance)!;
        await Assert.That(retained.Cast<string>()).IsEmpty();
        options.GetProperty("Fallback")!.SetValue(instance, "fallback");
        await Assert.That(validation.Validate(new(instance))).IsEmpty();

        options.GetProperty("Fallback")!.SetValue(instance, null);
        options.GetProperty("Values")!.SetValue(instance, System.Collections.Immutable.ImmutableArray<string>.Empty);
        await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        options.GetProperty("Values")!.SetValue(instance, System.Collections.Immutable.ImmutableArray.Create("first", "second"));
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
        retained = (IEnumerable) options.GetProperty("Values")!.GetValue(instance)!;
        await Assert.That(retained.Cast<string>().ToArray()).IsEquivalentTo(["first", "second"]);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Alternative_Collections_Retain_SingleUse_Inputs_After_Validation(bool positional)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, "IEnumerable<string>?"))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("Values")!;
        var source = new SingleUseAlternativeValues();
        property.SetValue(instance, source);
        var validation = (IValidatableObject) instance;

        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(validation.Validate(new(instance))).IsEmpty();
            var retained = ((IEnumerable) property.GetValue(instance)!).Cast<string>().ToArray();
            await Assert.That(retained).IsEquivalentTo(["first", "second"]);
            await Assert.That(RenderAlternativeCollection(instance, positional))
                .IsEquivalentTo(positional ? new[] { "first", "second" } : new[] { "--requirement", "first", "--requirement", "second" });
        }

        await Assert.That(source.EnumerationCount).IsEqualTo(1);
        property.SetValue(instance, new SingleUseAlternativeValues());
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
        await Assert.That(((IEnumerable) property.GetValue(instance)!).Cast<string>().ToArray())
            .IsEquivalentTo(["first", "second"]);
        property.SetValue(instance, Array.Empty<string>());
        await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        property.SetValue(instance, null);
        await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        options.GetProperty("Fallback")!.SetValue(instance, "fallback");
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
    }

    [Test]
    [Arguments(false, "IList<string>?")]
    [Arguments(true, "IList<string>?")]
    [Arguments(false, "ICollection<string>?")]
    [Arguments(true, "ICollection<string>?")]
    [Arguments(false, "System.Collections.IList?")]
    [Arguments(true, "System.Collections.IList?")]
    public async Task Alternative_Mutable_Interfaces_Preserve_Mutations(bool positional, string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("Values")!;
        List<string> supplied = ["first", "second"];
        property.SetValue(instance, supplied);
        supplied.Clear();
        var retained = property.GetValue(instance)!;
        if (retained is ICollection<string> collection)
        {
            collection.Remove("first");
            collection.Add("third");
        }
        else
        {
            ((IList) retained).Remove("first");
            ((IList) retained).Add("third");
        }

        var validation = (IValidatableObject) instance;
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
        await Assert.That(RenderAlternativeCollection(instance, positional))
            .IsEquivalentTo(positional ? new[] { "second", "third" } : new[] { "--requirement", "second", "--requirement", "third" });
        if (retained is ICollection<string> mutableCollection)
        {
            mutableCollection.Clear();
        }
        else
        {
            ((IList) retained).Clear();
        }

        await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        await Assert.That(RenderAlternativeCollection(instance, positional)).IsEmpty();
    }

    [Test]
    [Arguments("List<string>?")]
    [Arguments("string[]?")]
    [Arguments("IReadOnlyList<string>?")]
    [Arguments("ISet<string>?")]
    [Arguments("System.Collections.Immutable.ImmutableArray<string>?")]
    [Arguments("System.Collections.IEnumerable?")]
    public async Task Alternative_Collections_Preserve_Collection_Contracts(string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(false, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("Values")!;
        List<string> supplied = ["first", "second"];
        object input = collectionType switch
        {
            "string[]?" => supplied.ToArray(),
            "ISet<string>?" => new HashSet<string>(supplied, StringComparer.OrdinalIgnoreCase),
            "System.Collections.Immutable.ImmutableArray<string>?" => System.Collections.Immutable.ImmutableArray.CreateRange(supplied),
            _ => supplied,
        };
        property.SetValue(instance, input);
        supplied.Clear();
        var validation = (IValidatableObject) instance;
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
        var retained = (IEnumerable) property.GetValue(instance)!;
        await Assert.That(retained.Cast<string>().ToArray()).IsEquivalentTo(["first", "second"]);
        if (retained is HashSet<string> set)
        {
            await Assert.That(set.Comparer).IsSameReferenceAs(StringComparer.OrdinalIgnoreCase);
            set.Clear();
            await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        }
        else if (retained is List<string> list)
        {
            list.Clear();
            await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        }
    }

    [Test]
    [Arguments(false, "ISet<string>?")]
    [Arguments(true, "ISet<string>?")]
    [Arguments(false, "SortedSet<string>?")]
    [Arguments(true, "Queue<string>?")]
    public async Task Alternative_Collections_Accept_Existing_Concrete_Implementations(bool positional, string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("Values")!;
        object supplied = collectionType == "Queue<string>?"
            ? new Queue<string>(["first", "second"])
            : new SortedSet<string>(["first", "second"], StringComparer.OrdinalIgnoreCase);
        property.SetValue(instance, supplied);
        await Assert.That(property.GetValue(instance)).IsSameReferenceAs(supplied);
        var validation = (IValidatableObject) instance;
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(validation.Validate(new(instance))).IsEmpty();
            await Assert.That(((IEnumerable) property.GetValue(instance)!).Cast<string>().ToArray())
                .IsEquivalentTo(["first", "second"]);
        }

        if (supplied is SortedSet<string> set)
        {
            await Assert.That(set.Comparer).IsSameReferenceAs(StringComparer.OrdinalIgnoreCase);
            set.Clear();
        }
        else
        {
            ((Queue<string>) supplied).Clear();
        }

        await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        options.GetProperty("Fallback")!.SetValue(instance, "fallback");
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Alternative_Unresolved_Collections_Require_A_Known_Retention_Contract(bool positional)
    {
        var exception = await Assert.That(async () =>
        {
            await GenerateAlternativeCollection(positional, "PrivatePackage.SingleUseValues?", isCollection: true);
        }).Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).Contains("PrivatePackage.SingleUseValues?");
        await Assert.That(exception.Message).Contains("reusable snapshot");
    }

    [Test]
    [Arguments("List<string>?", false)]
    public async Task Alternative_Collections_Use_Explicit_Shape_Only_When_Type_Is_Unresolved(
        string collectionType, bool isCollection)
    {
        var generated = await GenerateAlternativeCollection(false, collectionType, isCollection);
        var assembly = Compile(generated);
        var options = assembly.GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("Values")!;
        var input = (IList) Activator.CreateInstance(property.PropertyType)!;
        property.SetValue(instance, input);
        var validation = (IValidatableObject) instance;
        await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        options.GetProperty("Fallback")!.SetValue(instance, "fallback");
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
        options.GetProperty("Fallback")!.SetValue(instance, null);
        input.Add("first");
        input.Add("second");
        property.SetValue(instance, input);
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(validation.Validate(new(instance))).IsEmpty();
            await Assert.That(((IEnumerable) property.GetValue(instance)!).Cast<string>())
                .IsEquivalentTo(["first", "second"]);
        }
    }

    [Test]
    [Arguments("IReadOnlyList<KeyValue>?")]
    [Arguments("List<KeyValue>?")]
    public async Task Alternative_Collections_Retain_Domain_Value_Types(string collectionType)
    {
        var assembly = Compile(await GenerateAlternativeCollection(false, collectionType));
        var options = assembly.GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var elementType = typeof(ModularPipelines.Models.KeyValue);
        var values = Array.CreateInstance(elementType, 2);
        values.SetValue(new ModularPipelines.Models.KeyValue("first", "value"), 0);
        values.SetValue(new ModularPipelines.Models.KeyValue("second", "value"), 1);
        var supplied = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType), [values]);
        var instance = Activator.CreateInstance(options)!;
        options.GetProperty("Values")!.SetValue(instance, supplied);
        var validation = (IValidatableObject) instance;
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
        var retained = (IEnumerable) options.GetProperty("Values")!.GetValue(instance)!;
        await Assert.That(retained.Cast<object>().ToArray()).IsEquivalentTo(values.Cast<object>());
    }

    [Test]
    [Arguments(false, "System.Collections.IEnumerable?")]
    [Arguments(true, "System.Collections.IEnumerable?")]
    [Arguments(false, "IEnumerable<char>?")]
    [Arguments(true, "IEnumerable<char>?")]
    public async Task Alternative_Enumerable_Contracts_Preserve_String_Inputs(bool positional, string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("Values")!;
        const string input = "complete-value";
        property.SetValue(instance, input);
        await Assert.That(property.GetValue(instance)).IsSameReferenceAs(input);
        var validation = (IValidatableObject) instance;
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
        property.SetValue(instance, string.Empty);
        await Assert.That(property.GetValue(instance)).IsSameReferenceAs(string.Empty);
        await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        options.GetProperty("Fallback")!.SetValue(instance, "fallback");
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
    }
    [Test]
    [Arguments(false, "System.Collections.IEnumerable?")]
    [Arguments(true, "System.Collections.IEnumerable?")]
    [Arguments(false, "IEnumerable<char>?")]
    [Arguments(true, "IEnumerable<char>?")]
    [Arguments(false, "IReadOnlyList<char>?")]
    [Arguments(true, "IReadOnlyList<char>?")]
    [Arguments(false, "List<char>?")]
    [Arguments(true, "List<char>?")]
    public async Task Alternative_Character_Sequences_Retain_Scalar_Rendering(bool positional, string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("Values")!;
        var input = new ScalarCharacterSequence();
        property.SetValue(instance, input);
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(property.GetValue(instance)).IsSameReferenceAs(input);
            await Assert.That(property.GetValue(instance)!.ToString()).IsEqualTo("scalar-value");
            await Assert.That(RenderAlternativeCollection(instance, positional))
                .IsEquivalentTo(positional ? new[] { "scalar-value" } : new[] { "--requirement", "scalar-value" });
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
        }
    }

    [Test]
    [Arguments("IEnumerable<object>?")]
    [Arguments("IReadOnlyList<object>?")]
    [Arguments("IReadOnlyCollection<object>?")]
    [Arguments("object[]?")]
    [Arguments("System.Collections.IEnumerable?")]
    [Arguments("System.Collections.ICollection?")]
    [Arguments("System.Collections.IList?")]
    public async Task Alternative_Broad_Collections_Preserve_Value_Pair_Rendering(string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(false, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("Values")!;
        ModularPipelines.Models.CliValuePair[] input = [new("first", "second"), new("third", "fourth")];
        property.SetValue(instance, collectionType == "object[]?" ? input : input.ToList());
        input[0] = new("changed", "input");
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            await Assert.That(RenderAlternativeCollection(instance, false))
                .IsEquivalentTo(["--requirement", "first", "second", "--requirement", "third", "fourth"]);
        }

        if (collectionType == "System.Collections.IList?")
        {
            var retained = (IList) property.GetValue(instance)!;
            retained.Clear();
            retained.Add(new ModularPipelines.Models.CliValuePair("fifth", "sixth"));
            await Assert.That(RenderAlternativeCollection(instance, false))
                .IsEquivalentTo(["--requirement", "fifth", "sixth"]);
        }
    }

    [Test]
    [Arguments("IEnumerable<object>?")]
    [Arguments("System.Collections.IEnumerable?")]
    public async Task Alternative_Value_Pairs_Enumerate_Once(string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(false, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new SingleUseValuePairs();
        options.GetProperty("Values")!.SetValue(instance, input);
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            await Assert.That(RenderAlternativeCollection(instance, false))
                .IsEquivalentTo(["--requirement", "first", "second"]);
        }

        await Assert.That(input.EnumerationCount).IsEqualTo(1);
    }

    [Test]
    [Arguments("IEnumerable<object>?")]
    [Arguments("IReadOnlyList<object>?")]
    [Arguments("System.Collections.IEnumerable?")]
    [Arguments("System.Collections.IList?")]
    public async Task Alternative_Default_Value_Pair_Arrays_Allow_Fallback(string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(false, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        options.GetProperty("Values")!.SetValue(instance, default(System.Collections.Immutable.ImmutableArray<ModularPipelines.Models.CliValuePair>));
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).Count().IsEqualTo(1);
        await Assert.That(RenderAlternativeCollection(instance, false)).IsEmpty();
        options.GetProperty("Fallback")!.SetValue(instance, "fallback");
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
    }

    [Test]
    [Arguments("IList<object>?")]
    [Arguments("ICollection<object>?")]
    [Arguments("List<object>?")]
    [Arguments("System.Collections.IList?")]
    public async Task Alternative_Mutable_Object_Collections_Snapshot_The_Pair_Interface(string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(false, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new SingleUseMutablePairs();
        options.GetProperty("Values")!.SetValue(instance, input);
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            await Assert.That(RenderAlternativeCollection(instance, false))
                .IsEquivalentTo(["--requirement", "first", "second"]);
        }

        await Assert.That(input.EnumerationCount).IsEqualTo(1);
        var retained = (IList) options.GetProperty("Values")!.GetValue(instance)!;
        retained.Clear();
        retained.Add(new ModularPipelines.Models.CliValuePair("third", "fourth"));
        await Assert.That(RenderAlternativeCollection(instance, false))
            .IsEquivalentTo(["--requirement", "third", "fourth"]);
        retained.Add("ordinary object addition");
        retained.Add(null!);
        await Assert.That(retained.Count).IsEqualTo(3);
        await Assert.That(RenderAlternativeCollection(instance, false))
            .IsEquivalentTo(["--requirement", "third", "fourth"]);
        retained.Remove(new ModularPipelines.Models.CliValuePair("third", "fourth"));
        await Assert.That(retained.Count).IsEqualTo(2);
        await Assert.That(RenderAlternativeCollection(instance, false)).IsEmpty();
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).Count().IsEqualTo(1);
        retained.Clear();
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).Count().IsEqualTo(1);
        await Assert.That(RenderAlternativeCollection(instance, false)).IsEmpty();
    }

    [Test]
    [Arguments("IList<object>?")]
    [Arguments("ICollection<object>?")]
    [Arguments("List<object>?")]
    [Arguments("IEnumerable<object>?")]
    [Arguments("System.Collections.IEnumerable?")]
    [Arguments("System.Collections.IList?")]
    public async Task Alternative_Positional_Collections_Use_The_Ordinary_Enumeration(string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(true, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new SingleUseMutablePairs();
        options.GetProperty("Values")!.SetValue(instance, input);
        input.Clear();
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            await Assert.That(RenderAlternativeCollection(instance, true)).IsEquivalentTo(["ordinary object view"]);
        }

        await Assert.That(input.EnumerationCount).IsEqualTo(0);
    }

    [Test]
    [Arguments(false, "IEnumerable<object>?")]
    [Arguments(true, "IEnumerable<object>?")]
    [Arguments(false, "IReadOnlyList<object>?")]
    [Arguments(true, "IReadOnlyList<object>?")]
    [Arguments(false, "IList<object>?")]
    [Arguments(true, "IList<object>?")]
    [Arguments(false, "ICollection<object>?")]
    [Arguments(true, "ICollection<object>?")]
    [Arguments(false, "List<object>?")]
    [Arguments(true, "List<object>?")]
    [Arguments(false, "System.Collections.IList?")]
    [Arguments(true, "System.Collections.IList?")]
    [Arguments(false, "System.Collections.IEnumerable?")]
    [Arguments(true, "System.Collections.IEnumerable?")]
    [Arguments(false, "System.Collections.ICollection?")]
    [Arguments(true, "System.Collections.ICollection?")]
    public async Task Alternative_Collections_Snapshot_The_KeyValue_Interface(bool positional, string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("Values")!;
        var input = new SingleUseMutableKeyValues();
        property.SetValue(instance, input);
        input.Clear();
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            await Assert.That(RenderAlternativeCollection(instance, positional))
                .IsEquivalentTo(positional ? new[] { "first=second" } : ["--requirement", "first=second"]);
        }

        await Assert.That(input.EnumerationCount).IsEqualTo(1);
        if (property.GetValue(instance) is IList { IsFixedSize: false } retained)
        {
            retained.Clear();
            var pair = new ModularPipelines.Models.KeyValue("third", "fourth", ":");
            retained.Add(pair);
            retained.Add("ordinary object addition");
            retained.Add(null!);
            await Assert.That(retained.Count).IsEqualTo(3);
            await Assert.That(RenderAlternativeCollection(instance, positional))
                .IsEquivalentTo(positional ? new[] { "third:fourth" } : ["--requirement", "third:fourth"]);
            retained.Remove(pair);
            await Assert.That(retained.Count).IsEqualTo(2);
            await Assert.That(RenderAlternativeCollection(instance, positional)).IsEmpty();
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).Count().IsEqualTo(1);
        }
    }

    [Test]
    [Arguments(false, "IEnumerable<object>?")]
    [Arguments(true, "IEnumerable<object>?")]
    [Arguments(false, "IReadOnlyList<object>?")]
    [Arguments(true, "IReadOnlyList<object>?")]
    [Arguments(false, "System.Collections.IEnumerable?")]
    [Arguments(true, "System.Collections.IEnumerable?")]
    [Arguments(false, "System.Collections.IList?")]
    [Arguments(true, "System.Collections.IList?")]
    public async Task Alternative_Default_KeyValue_Arrays_Allow_Fallback(bool positional, string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        options.GetProperty("Values")!.SetValue(instance, default(System.Collections.Immutable.ImmutableArray<ModularPipelines.Models.KeyValue>));
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).Count().IsEqualTo(1);
        await Assert.That(RenderAlternativeCollection(instance, positional)).IsEmpty();
        options.GetProperty("Fallback")!.SetValue(instance, "fallback");
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task Alternative_Snapshots_Follow_Runtime_View_Precedence(bool positional, bool characterView)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, "IList<object>?"))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = characterView ? new CharacterAndPairViews() : new PairAndKeyValueViews();
        options.GetProperty("Values")!.SetValue(instance, input);
        string[] expected = positional
            ? [characterView ? "scalar-value" : "first=second"]
            : ["--requirement", "pair-first", "pair-second"];
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            await Assert.That(RenderAlternativeCollection(instance, positional)).IsEquivalentTo(expected);
        }

        await Assert.That(input.PairEnumerationCount).IsEqualTo(positional ? 0 : 1);
        await Assert.That(input.EnumerationCount).IsEqualTo(positional && !characterView ? 1 : 0);
    }

    [Test]
    [Arguments(false, "ISet<object>?", "reference")]
    [Arguments(true, "ISet<object>?", "reference")]
    [Arguments(false, "IReadOnlySet<object>?", "reference")]
    [Arguments(true, "IReadOnlySet<object>?", "reference")]
    [Arguments(false, "HashSet<object>?", "reference")]
    [Arguments(true, "HashSet<object>?", "reference")]
    [Arguments(false, "ISet<object>?", "all-equal")]
    [Arguments(true, "ISet<object>?", "all-equal")]
    [Arguments(false, "IReadOnlySet<object>?", "all-equal")]
    [Arguments(true, "IReadOnlySet<object>?", "all-equal")]
    [Arguments(false, "HashSet<object>?", "all-equal")]
    [Arguments(true, "HashSet<object>?", "all-equal")]
    [Arguments(false, "ISet<object>?", "strings-only")]
    [Arguments(true, "ISet<object>?", "strings-only")]
    [Arguments(false, "IReadOnlySet<object>?", "strings-only")]
    [Arguments(true, "IReadOnlySet<object>?", "strings-only")]
    [Arguments(false, "HashSet<object>?", "strings-only")]
    [Arguments(true, "HashSet<object>?", "strings-only")]
    public async Task Alternative_Custom_Set_Views_Retain_Their_Membership_Contract(bool positional, string collectionType, string comparer)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new IndependentSetViews(comparer);
        options.GetProperty("Values")!.SetValue(instance, input);
        var retained = (IReadOnlySet<object>) options.GetProperty("Values")!.GetValue(instance)!;
        string[] expected = positional ? ["first=second", "first=second"] : ["--requirement", "first", "second", "--requirement", "first", "second"];
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            await Assert.That(RenderAlternativeCollection(instance, positional)).IsEquivalentTo(expected);
        }

        if (collectionType != "IReadOnlySet<object>?")
        {
            await Assert.That(retained).IsSameReferenceAs(input);
            await Assert.That(((HashSet<object>) retained).Comparer).IsSameReferenceAs(input.Comparer);
        }

        await Assert.That(retained.SetEquals((IEnumerable<object>) input)).IsTrue();
        // This custom source defines its typed view independently of ordinary membership.
        // Mutating its object set must retain that behavior rather than invent a new mapping.
        input.Clear();
        input.Add("ordinary object addition");
        await Assert.That(retained.Contains("ordinary object addition")).IsTrue();
        await Assert.That(RenderAlternativeCollection(instance, positional)).IsEquivalentTo(expected);
        await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
    }

    [Test]
    [Arguments(false, "IEnumerable<string>?")]
    [Arguments(true, "IEnumerable<string>?")]
    [Arguments(false, "List<string>?")]
    [Arguments(true, "List<string>?")]
    [Arguments(false, "IEnumerable<KeyValue>?")]
    [Arguments(false, "List<KeyValue>?")]
    [Arguments(true, "IEnumerable<CliValuePair>?")]
    [Arguments(true, "List<CliValuePair>?")]
    [Arguments(false, "System.Collections.ArrayList?")]
    [Arguments(true, "System.Collections.ArrayList?")]
    public async Task Alternative_Incompatible_Typed_Views_Retain_Their_Source(bool positional, string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        object input = collectionType switch
        {
            "IEnumerable<string>?" or "List<string>?" => new StringRendererViews(),
            "IEnumerable<KeyValue>?" or "List<KeyValue>?" => new KeyValueListWithPairView(),
            "IEnumerable<CliValuePair>?" or "List<CliValuePair>?" => new PairListWithKeyValueView(),
            _ => new ArrayListRendererViews(),
        };
        var property = options.GetProperty("Values")!;
        property.SetValue(instance, input);
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            string[] expected = positional ? ["typed=value"] : ["--requirement", "typed", "value"];
            await Assert.That(RenderAlternativeCollection(instance, positional)).IsEquivalentTo(expected);
        }

        if (!collectionType.StartsWith("IEnumerable<", StringComparison.Ordinal))
        {
            await Assert.That(property.GetValue(instance)).IsSameReferenceAs(input);
        }
    }

    [Test]
    [Arguments(false, "IEnumerable<KeyValue>?")]
    [Arguments(true, "IEnumerable<KeyValue>?")]
    [Arguments(false, "IEnumerable<global::ModularPipelines.Models.KeyValue>?")]
    [Arguments(true, "IEnumerable<global::ModularPipelines.Models.KeyValue>?")]
    [Arguments(false, "IEnumerable<KeyValue?>?")]
    [Arguments(true, "IEnumerable<KeyValue?>?")]
    public async Task Alternative_Matching_Typed_Views_Still_Snapshot_Once(bool positional, string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new SingleUseKeyValues();
        options.GetProperty("Values")!.SetValue(instance, input);
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            string[] expected = positional ? ["typed=value"] : ["--requirement", "typed=value"];
            await Assert.That(RenderAlternativeCollection(instance, positional)).IsEquivalentTo(expected);
        }

        await Assert.That(input.EnumerationCount).IsEqualTo(1);
    }

    private sealed class SingleUseKeyValues : IEnumerable<ModularPipelines.Models.KeyValue>
    {
        public int EnumerationCount { get; private set; }

        public IEnumerator<ModularPipelines.Models.KeyValue> GetEnumerator()
        {
            if (++EnumerationCount != 1)
            {
                throw new InvalidOperationException("KeyValue input can only be enumerated once.");
            }

            yield return new("typed", "value");
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private static IEnumerator<ModularPipelines.Models.CliValuePair> CreatePairView()
    {
        yield return new("typed", "value");
    }

    private static IEnumerator<ModularPipelines.Models.KeyValue> CreateKeyValueView()
    {
        yield return new("typed", "value");
    }
    [Test]
    [Arguments(false, "IEnumerable<string>?")]
    [Arguments(true, "IEnumerable<string>?")]
    [Arguments(false, "IReadOnlyCollection<string>?")]
    [Arguments(true, "IReadOnlyCollection<string>?")]
    [Arguments(false, "IReadOnlyList<string>?")]
    [Arguments(true, "IReadOnlyList<string>?")]
    public async Task Alternative_ReadOnly_Contracts_Snapshot_A_SingleUse_Renderer_View(bool positional, string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new StringRendererViews(singleUse: true);
        var property = options.GetProperty("Values")!;
        property.SetValue(instance, input);
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            string[] expected = positional ? ["typed=value"] : ["--requirement", "typed", "value"];
            await Assert.That(RenderAlternativeCollection(instance, positional)).IsEquivalentTo(expected);
            await Assert.That((IEnumerable<string>) property.GetValue(instance)!).IsEquivalentTo(["ordinary"]);
        }

        if (property.GetValue(instance) is IReadOnlyCollection<string> collection)
        {
            await Assert.That(collection.Count).IsEqualTo(1);
        }

        if (property.GetValue(instance) is IReadOnlyList<string> list)
        {
            await Assert.That(list[0]).IsEqualTo("ordinary");
        }

        await Assert.That(input.PairEnumerationCount).IsEqualTo(positional ? 0 : 1);
        await Assert.That(input.KeyValueEnumerationCount).IsEqualTo(positional ? 1 : 0);
    }

    private sealed class StringRendererViews : List<string>, IEnumerable<ModularPipelines.Models.CliValuePair>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        private readonly bool _singleUse;

        public StringRendererViews(bool singleUse = false)
        {
            _singleUse = singleUse;
            Add("ordinary");
        }

        public int PairEnumerationCount { get; private set; }

        public int KeyValueEnumerationCount { get; private set; }

        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator()
        {
            if (++PairEnumerationCount > 1 && _singleUse)
            {
                throw new InvalidOperationException("Pair input can only be enumerated once.");
            }

            yield return new("typed", "value");
        }

        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator()
        {
            if (++KeyValueEnumerationCount > 1 && _singleUse)
            {
                throw new InvalidOperationException("KeyValue input can only be enumerated once.");
            }

            yield return new("typed", "value");
        }
    }

    private sealed class KeyValueListWithPairView : List<ModularPipelines.Models.KeyValue>, IEnumerable<ModularPipelines.Models.CliValuePair>
    {
        public KeyValueListWithPairView() => Add(new("ordinary", "value"));

        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator() =>
            CreatePairView();
    }

    private sealed class PairListWithKeyValueView : List<ModularPipelines.Models.CliValuePair>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        public PairListWithKeyValueView() => Add(new("ordinary", "value"));

        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator() =>
            CreateKeyValueView();
    }

    private sealed class ArrayListRendererViews : ArrayList, IEnumerable<ModularPipelines.Models.CliValuePair>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        public ArrayListRendererViews() => Add("ordinary");

        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator() =>
            CreatePairView();

        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator() =>
            CreateKeyValueView();
    }

    [Test]
    [Arguments(false, "IReadOnlySet<string>?")]
    [Arguments(true, "IReadOnlySet<string>?")]
    [Arguments(false, "global::System.Collections.Generic.IReadOnlySet<string>?")]
    [Arguments(true, "global::System.Collections.Generic.IReadOnlySet<string>?")]
    [Arguments(false, "IReadOnlySet<object>?")]
    [Arguments(true, "IReadOnlySet<object>?")]
    public async Task Alternative_ReadOnly_Sets_Snapshot_SingleUse_Renderer_Views(bool positional, string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var objectSet = collectionType.Contains("<object>", StringComparison.Ordinal);
        object input = objectSet ? new IndependentSetViews("strings-only", singleUse: true) : new SingleUseStringSetViews();
        var property = options.GetProperty("Values")!;
        property.SetValue(instance, input);
        string[] expected = (objectSet, positional) switch
        {
            (true, true) => ["first=second", "first=second"],
            (true, false) => ["--requirement", "first", "second", "--requirement", "first", "second"],
            (false, true) => ["typed=value"],
            (false, false) => ["--requirement", "typed", "value"],
        };
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            await Assert.That(RenderAlternativeCollection(instance, positional)).IsEquivalentTo(expected);
        }

        if (input is SingleUseStringSetViews strings)
        {
            var retained = (IReadOnlySet<string>) property.GetValue(instance)!;
            await Assert.That(retained.Count).IsEqualTo(1);
            await Assert.That(retained.Contains("ORDINARY")).IsTrue();
            await Assert.That(retained.SetEquals(["ORDINARY"])).IsTrue();
            await Assert.That(retained.IsSubsetOf(["ORDINARY"])).IsTrue();
            await Assert.That(retained.IsProperSubsetOf(["ORDINARY", "other"])).IsTrue();
            await Assert.That(retained.IsSupersetOf(["ORDINARY"])).IsTrue();
            await Assert.That(retained.IsProperSupersetOf([])).IsTrue();
            await Assert.That(retained.Overlaps(["ORDINARY"])).IsTrue();
            await Assert.That(strings.Renderer.PairEnumerationCount).IsEqualTo(positional ? 0 : 1);
            await Assert.That(strings.Renderer.KeyValueEnumerationCount).IsEqualTo(positional ? 1 : 0);
        }
        else
        {
            var objects = (IndependentSetViews) input;
            var retained = (IReadOnlySet<object>) property.GetValue(instance)!;
            await Assert.That(retained.Count).IsEqualTo(1);
            await Assert.That(retained.Contains("ordinary object view")).IsTrue();
            await Assert.That(objects.PairEnumerationCount).IsEqualTo(positional ? 0 : 1);
            await Assert.That(objects.KeyValueEnumerationCount).IsEqualTo(positional ? 1 : 0);
        }
    }

    private sealed class SingleUseStringSetViews : HashSet<string>, IEnumerable<ModularPipelines.Models.CliValuePair>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        public SingleUseStringSetViews() : base(StringComparer.OrdinalIgnoreCase) => Add("ordinary");

        public StringRendererViews Renderer { get; } = new(singleUse: true);

        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator() =>
            ((IEnumerable<ModularPipelines.Models.CliValuePair>) Renderer).GetEnumerator();

        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator() =>
            ((IEnumerable<ModularPipelines.Models.KeyValue>) Renderer).GetEnumerator();
    }

    private sealed class IndependentSetViews : HashSet<object>, IEnumerable<ModularPipelines.Models.CliValuePair>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        private readonly bool _singleUse;

        public IndependentSetViews(string comparer, bool singleUse = false) : base(comparer switch
        {
            "reference" => ReferenceEqualityComparer.Instance,
            "all-equal" => EqualityComparer<object>.Create(static (_, _) => true, static _ => 0),
            _ => EqualityComparer<object>.Create(static (left, right) => (string) left! == (string) right!, static value => ((string) value).GetHashCode()),
        })
        {
            _singleUse = singleUse;
            Add("ordinary object view");
        }

        public int PairEnumerationCount { get; private set; }

        public int KeyValueEnumerationCount { get; private set; }

        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator()
        {
            if (++PairEnumerationCount > 1 && _singleUse)
            {
                throw new InvalidOperationException("Pair input can only be enumerated once.");
            }

            var pair = new ModularPipelines.Models.CliValuePair("first", "second");
            yield return pair;
            yield return pair;
        }

        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator()
        {
            if (++KeyValueEnumerationCount > 1 && _singleUse)
            {
                throw new InvalidOperationException("KeyValue input can only be enumerated once.");
            }

            var pair = new ModularPipelines.Models.KeyValue("first", "second");
            yield return pair;
            yield return pair;
        }
    }

    private class PairAndKeyValueViews : SingleUseMutableKeyValues, IEnumerable<ModularPipelines.Models.CliValuePair>
    {
        public int PairEnumerationCount { get; private set; }

        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator()
        {
            if (++PairEnumerationCount != 1)
            {
                throw new InvalidOperationException("Pair input can only be enumerated once.");
            }

            yield return new("pair-first", "pair-second");
        }
    }

    private sealed class CharacterAndPairViews : PairAndKeyValueViews, IEnumerable<char>
    {
        public override string ToString() => "scalar-value";

        IEnumerator<char> IEnumerable<char>.GetEnumerator() =>
            throw new InvalidOperationException("Scalar character sequences must not be enumerated.");
    }

    private class SingleUseMutableKeyValues : List<object>, IEnumerable<ModularPipelines.Models.KeyValue>
    {
        public int EnumerationCount { get; private set; }

        public SingleUseMutableKeyValues() => Add("ordinary object view");

        IEnumerator<ModularPipelines.Models.KeyValue> IEnumerable<ModularPipelines.Models.KeyValue>.GetEnumerator()
        {
            if (++EnumerationCount != 1)
            {
                throw new InvalidOperationException("KeyValue input can only be enumerated once.");
            }

            yield return new("first", "second");
        }
    }

    private sealed class SingleUseMutablePairs : List<object>, IEnumerable<ModularPipelines.Models.CliValuePair>
    {
        public int EnumerationCount { get; private set; }

        // The object view intentionally differs from the pair view. Rendering and
        // snapshots must use the specialized interface selected by the command builder.
        public SingleUseMutablePairs() => Add("ordinary object view");

        IEnumerator<ModularPipelines.Models.CliValuePair> IEnumerable<ModularPipelines.Models.CliValuePair>.GetEnumerator()
        {
            if (++EnumerationCount != 1)
            {
                throw new InvalidOperationException("Pair input can only be enumerated once.");
            }

            yield return new("first", "second");
        }
    }

    private sealed class SingleUseValuePairs : IEnumerable<ModularPipelines.Models.CliValuePair>
    {
        public int EnumerationCount { get; private set; }

        public IEnumerator<ModularPipelines.Models.CliValuePair> GetEnumerator()
        {
            if (++EnumerationCount != 1)
            {
                throw new InvalidOperationException("Input can only be enumerated once.");
            }

            yield return new("first", "second");
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class ScalarCharacterSequence : List<char>, IEnumerable<char>
    {
        public override string ToString() => "scalar-value";

        IEnumerator<char> IEnumerable<char>.GetEnumerator() =>
            throw new InvalidOperationException("Scalar character sequences must not be enumerated.");

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<char>) this).GetEnumerator();
    }
    private static IReadOnlyList<string> RenderAlternativeCollection(object instance, bool positional)
    {
        var property = instance.GetType().GetProperty("Values")!;
        PropertyCommandLinePart part = positional
            ? new ArgumentPart("Values", property.GetValue, new CliArgumentAttribute(0))
            : new OptionPart("Values", property.GetValue, new CliOptionAttribute("--requirement"));
        return new CommandArgumentBuilder().BuildArguments([part], instance);
    }
    private static Task<string> GenerateAlternativeCollection(bool positional, string collectionType, bool? isCollection = null)
    {
        List<CliOptionDefinition> options =
            [new() { SwitchName = "--fallback", PropertyName = "Fallback", CSharpType = "string?" }];
        if (!positional)
        {
            options.Add(new() { SwitchName = "--requirement", PropertyName = "Values", CSharpType = collectionType, IsCollection = isCollection });
        }

        var member = new CliRequiredAlternativeMember
        {
            PropertyName = "Values",
            OptionSwitch = positional ? null : "--requirement",
            PositionalArgumentPhase = positional ? ModularPipelines.Attributes.CommandLinePhase.EarlyOperand : null,
            PositionalArgumentPositionIndex = positional ? 0 : null,
        };
        return Generate(options,
            positional ? [new() { PropertyName = "Values", CSharpType = collectionType, PositionIndex = 0, IsVariadic = isCollection == true }] : [],
            [new() { Members = [member, new() { PropertyName = "Fallback", OptionSwitch = "--fallback" }] }]);
    }

    private sealed class SingleUseAlternativeValues : IEnumerable<string>
    {
        public int EnumerationCount { get; private set; }

        public IEnumerator<string> GetEnumerator()
        {
            if (++EnumerationCount != 1)
            {
                throw new InvalidOperationException("Input can only be enumerated once.");
            }

            return ((IEnumerable<string>) new[] { "first", "second" }).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
