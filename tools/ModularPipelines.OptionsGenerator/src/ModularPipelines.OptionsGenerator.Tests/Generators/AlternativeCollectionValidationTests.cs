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
