using System.Collections;
using System.ComponentModel.DataAnnotations;
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
    [Arguments("PrivatePackage.CustomValues?", true)]
    [Arguments("List<string>?", false)]
    public async Task Alternative_Collections_Use_Explicit_Shape_Only_When_Type_Is_Unresolved(
        string collectionType, bool isCollection)
    {
        const string customCollection = """
            namespace PrivatePackage;
            public sealed class CustomValues : System.Collections.Generic.List<string>;
            """;
        var generated = await GenerateAlternativeCollection(false, collectionType, isCollection);
        var assembly = Compile(generated, customCollection);
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
        var elementType = assembly.GetType("ModularPipelines.Models.KeyValue")!;
        var values = Array.CreateInstance(elementType, 2);
        values.SetValue(Activator.CreateInstance(elementType), 0);
        values.SetValue(Activator.CreateInstance(elementType), 1);
        var supplied = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType), [values]);
        var instance = Activator.CreateInstance(options)!;
        options.GetProperty("Values")!.SetValue(instance, supplied);
        var validation = (IValidatableObject) instance;
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
        var retained = (IEnumerable) options.GetProperty("Values")!.GetValue(instance)!;
        await Assert.That(retained.Cast<object>().ToArray()).IsEquivalentTo(values.Cast<object>());
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
            positional ? [new() { PropertyName = "Values", CSharpType = collectionType, PositionIndex = 0 }] : [],
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
