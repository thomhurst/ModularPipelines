using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.TestHelpers;

/// <summary>
/// Smoke-tests command-line rendering for every options record in an integration assembly.
/// </summary>
public static class GeneratedOptionsSmokeTestHarness
{
    /// <summary>
    /// Validates every concrete <see cref="CommandLineToolOptions"/> type in an assembly.
    /// </summary>
    /// <param name="assembly">The integration assembly to validate.</param>
    /// <returns>The number of option types and attributed properties tested.</returns>
    public static GeneratedOptionsSmokeTestResult ValidateAssembly(Assembly assembly)
    {
        return ValidateAssembly(assembly, useReflectionFallback: false);
    }

    /// <summary>
    /// Validates every options type using the runtime reflection fallback.
    /// </summary>
    /// <param name="assembly">The integration assembly to validate.</param>
    /// <returns>The number of option types and attributed properties tested.</returns>
    public static GeneratedOptionsSmokeTestResult ValidateAssemblyUsingReflection(Assembly assembly)
    {
        return ValidateAssembly(assembly, useReflectionFallback: true);
    }

    private static GeneratedOptionsSmokeTestResult ValidateAssembly(
        Assembly assembly,
        bool useReflectionFallback)
    {
        var results = assembly.GetTypes()
            .Where(IsOptionsType)
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .Select(type => ValidateOptionsType(type, useReflectionFallback))
            .ToList();

        return new GeneratedOptionsSmokeTestResult(
            results.Count,
            results.Sum(result => result.PropertiesTested));
    }

    /// <summary>
    /// Validates every command-line property on one options type.
    /// </summary>
    /// <param name="optionsType">The options type to validate.</param>
    /// <returns>The number of option types and attributed properties tested.</returns>
    public static GeneratedOptionsSmokeTestResult ValidateOptionsType(Type optionsType)
    {
        return ValidateOptionsType(optionsType, useReflectionFallback: false);
    }

    /// <summary>
    /// Validates one options type using the runtime reflection fallback.
    /// </summary>
    /// <param name="optionsType">The options type to validate.</param>
    /// <returns>The number of option types and attributed properties tested.</returns>
    public static GeneratedOptionsSmokeTestResult ValidateOptionsTypeUsingReflection(Type optionsType)
    {
        return ValidateOptionsType(optionsType, useReflectionFallback: true);
    }

    private static GeneratedOptionsSmokeTestResult ValidateOptionsType(
        Type optionsType,
        bool useReflectionFallback)
    {
        if (!typeof(CommandLineToolOptions).IsAssignableFrom(optionsType))
        {
            throw new ArgumentException(
                $"{optionsType.FullName} does not derive from {nameof(CommandLineToolOptions)}.",
                nameof(optionsType));
        }

        var model = useReflectionFallback
            ? CommandModelProvider.GetReflectionCommandModel(optionsType)
            : new CommandModelProvider().GetCommandModel(optionsType);

        if (optionsType.IsAbstract)
        {
            return new GeneratedOptionsSmokeTestResult(1, 0);
        }

        var builder = new CommandArgumentBuilder();

        foreach (var part in model)
        {
            ValidatePart(optionsType, model, builder, part);
        }

        return new GeneratedOptionsSmokeTestResult(1, model.Count);
    }

    private static bool IsOptionsType(Type type) =>
        !type.ContainsGenericParameters
        && typeof(CommandLineToolOptions).IsAssignableFrom(type);

    private static void ValidatePart(
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> model,
        CommandArgumentBuilder builder,
        PropertyCommandLinePart part)
    {
        try
        {
            var property = optionsType
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .First(candidate => candidate.Name == part.PropertyName);
            var sample = CreateSample(property.PropertyType);
            var options = RuntimeHelpers.GetUninitializedObject(optionsType);

            SetValue(options, property, sample);

            var actual = builder.BuildArguments(model, options);
            var expected = GetExpectedArguments(model, options);

            if (!actual.SequenceEqual(expected, StringComparer.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Expected [{string.Join(", ", expected)}], " +
                    $"but rendered [{string.Join(", ", actual)}].");
            }
        }
        catch (Exception exception) when (exception is not GeneratedOptionsSmokeTestException)
        {
            throw new GeneratedOptionsSmokeTestException(
                optionsType,
                part.PropertyName,
                exception);
        }
    }

    private static IReadOnlyList<string> GetExpectedArguments(
        IReadOnlyList<PropertyCommandLinePart> model,
        object options)
    {
        var arguments = model.OfType<ArgumentPart>().ToList();
        var switches = model.Where(part => part is FlagPart or OptionPart).ToList();
        var expected = new List<string>();

        expected.AddRange(RenderArguments(
            arguments.Where(argument =>
                argument.Attribute.Placement == ArgumentPlacement.ImmediatelyAfterCommand),
            options));
        expected.AddRange(RenderArguments(
            arguments.Where(argument =>
                argument.Attribute.Placement == ArgumentPlacement.BeforeOptions),
            options));

        foreach (var phase in new[]
                 {
                     CommandLinePhase.Normal,
                     CommandLinePhase.EndOfOptions,
                     CommandLinePhase.Passthrough,
                     CommandLinePhase.Terminal,
                 })
        {
            var phaseArguments = arguments.Where(argument =>
                argument.Attribute.Placement == ArgumentPlacement.AfterOptions
                && argument.Phase == phase);
            var phaseSwitches = switches.Where(part => part.Phase == phase);

            if (phase == CommandLinePhase.Terminal)
            {
                expected.AddRange(RenderArguments(phaseArguments, options));
                expected.AddRange(RenderSwitches(phaseSwitches, options));
            }
            else
            {
                expected.AddRange(RenderSwitches(phaseSwitches, options));
                expected.AddRange(RenderArguments(phaseArguments, options));
            }
        }

        return expected;
    }

    private static IEnumerable<string> RenderArguments(
        IEnumerable<ArgumentPart> arguments,
        object options) =>
        arguments
            .OrderBy(argument => argument.Attribute.Position)
            .SelectMany(argument => GetExpectedArguments(argument, argument.Getter(options)));

    private static IEnumerable<string> RenderSwitches(
        IEnumerable<PropertyCommandLinePart> switches,
        object options) =>
        switches.SelectMany(part => GetExpectedArguments(part, part.Getter(options)));

    private static IReadOnlyList<string> GetExpectedArguments(
        PropertyCommandLinePart part,
        object? value)
    {
        if (value is null)
        {
            return [];
        }

        return part switch
        {
            ArgumentPart { Attribute.Name: not null } => [],
            ArgumentPart argument => GetExpectedArgument(argument.Attribute, value),
            FlagPart flag => GetExpectedFlag(flag.Attribute, value),
            OptionPart option => GetExpectedOption(option, value),
            _ => throw new ArgumentOutOfRangeException(nameof(part)),
        };
    }

    private static IReadOnlyList<string> GetExpectedFlag(
        CliFlagAttribute attribute,
        object value)
    {
        return value switch
        {
            true => [attribute.GetEffectiveName()],
            int count when count > 0 => Enumerable.Repeat(attribute.GetEffectiveName(), count).ToList(),
            _ => [],
        };
    }

    private static IReadOnlyList<string> GetExpectedArgument(
        CliArgumentAttribute attribute,
        object sample)
    {
        var values = GetValues(sample);

        return attribute.PrependOptionTerminator
            ? ["--", .. values]
            : values;
    }

    private static IReadOnlyList<string> GetExpectedOption(OptionPart option, object value)
    {
        var optionName = option.Attribute.GetEffectiveName();

        if (option.ValueArity == CliOptionValueArity.None)
        {
            return value is not false ? [optionName] : [];
        }

        if (value is CliOptionValuePair pair)
        {
            return [optionName, pair.First, pair.Second];
        }

        if (value is IEnumerable<CliOptionValuePair> pairs)
        {
            return pairs
                .SelectMany(pairValue => new[] { optionName, pairValue.First, pairValue.Second })
                .ToList();
        }

        var separator = option.Attribute.GetSeparator();
        return GetValues(value)
            .SelectMany(renderedValue => separator == " "
                ? new[] { optionName, renderedValue }
                : new[] { $"{optionName}{separator}{renderedValue}" })
            .ToList();
    }

    private static object CreateSample(Type propertyType)
    {
        var type = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (TryCreateKnownSample(type) is { } knownSample)
        {
            return knownSample;
        }

        if (type.IsEnum)
        {
            return CreateEnumSample(type);
        }

        if (IsNumeric(type))
        {
            return Convert.ChangeType(1, type, CultureInfo.InvariantCulture);
        }

        if (TryCreateEnumerableSample(type) is { } enumerableSample)
        {
            return enumerableSample;
        }

        return CreateConstructedSample(type);
    }

    private static object? TryCreateKnownSample(Type type) =>
        type == typeof(string) ? "smoke-value"
        : type == typeof(bool) ? true
        : type == typeof(CliOptionValuePair) ? new CliOptionValuePair("smoke-first", "smoke-second")
        : type == typeof(KeyValue) ? new KeyValue("smoke-key", "smoke-value")
        : type == typeof(Uri) ? new Uri("https://example.invalid/smoke")
        : null;

    private static object CreateEnumSample(Type type) =>
        Enum.GetValues(type).GetValue(0)
        ?? throw new InvalidOperationException($"{type.FullName} has no values.");

    private static object? TryCreateEnumerableSample(Type type)
    {
        if (TryGetEnumerableElementType(type) is not { } elementType)
        {
            return null;
        }

        var element = CreateSample(elementType);
        var array = Array.CreateInstance(elementType, 1);
        array.SetValue(element, 0);

        if (type.IsAssignableFrom(array.GetType()))
        {
            return array;
        }

        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = (IList) Activator.CreateInstance(listType)!;
        list.Add(element);

        return type.IsAssignableFrom(listType) ? list : null;
    }

    private static object CreateConstructedSample(Type type)
    {
        if (type.GetConstructor([typeof(string)]) is { } stringConstructor)
        {
            return stringConstructor.Invoke(["smoke-value"]);
        }

        if (Activator.CreateInstance(type) is { } instance)
        {
            return instance;
        }

        throw new InvalidOperationException($"No representative value is defined for {type.FullName}.");
    }

    private static Type? TryGetEnumerableElementType(Type type)
    {
        if (type.IsArray)
        {
            return type.GetElementType();
        }

        return type
            .GetInterfaces()
            .Append(type)
            .FirstOrDefault(candidate =>
                candidate.IsGenericType
                && candidate.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            ?.GetGenericArguments()[0];
    }

    private static bool IsNumeric(Type type) =>
        type == typeof(byte)
        || type == typeof(short)
        || type == typeof(ushort)
        || type == typeof(int)
        || type == typeof(uint)
        || type == typeof(long)
        || type == typeof(ulong)
        || type == typeof(float)
        || type == typeof(double)
        || type == typeof(decimal);

    private static void SetValue(object target, PropertyInfo property, object value)
    {
        if (property.SetMethod is not null)
        {
            property.SetValue(target, value);
            return;
        }

        var backingField = property.DeclaringType?.GetField(
            $"<{property.Name}>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic);

        if (backingField is null)
        {
            throw new InvalidOperationException($"{property.Name} cannot be assigned.");
        }

        backingField.SetValue(target, value);
    }

    private static IReadOnlyList<string> GetValues(object value) =>
        value switch
        {
            string stringValue => [stringValue],
            IEnumerable<KeyValue> keyValues => keyValues.Select(item => item.ToString()).ToList(),
            IEnumerable enumerable when value is not IEnumerable<char> => GetEnumerableValues(enumerable),
            bool boolValue => [boolValue.ToString().ToLowerInvariant()],
            Uri uri => [uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString()],
            IFormattable formattable when !value.GetType().IsEnum =>
                [formattable.ToString(null, CultureInfo.InvariantCulture)],
            _ => GetEnumOrDefaultValue(value),
        };

    private static IReadOnlyList<string> GetEnumerableValues(IEnumerable enumerable) =>
        enumerable
            .Cast<object>()
            .SelectMany(GetValues)
            .ToList();

    private static IReadOnlyList<string> GetEnumOrDefaultValue(object value)
    {
        var enumValue = value.GetType()
            .GetField(value.ToString()!)?
            .GetCustomAttribute<EnumValueAttribute>()?
            .Value;

        return [enumValue ?? value.ToString()!];
    }
}

/// <summary>
/// Counts the command-line options covered by a smoke-test run.
/// </summary>
/// <param name="OptionsTypesTested">The number of concrete options types tested.</param>
/// <param name="PropertiesTested">The number of attributed command-line properties tested.</param>
public sealed record GeneratedOptionsSmokeTestResult(
    int OptionsTypesTested,
    int PropertiesTested);

/// <summary>
/// Identifies the options type and property that failed command-line rendering.
/// </summary>
public sealed class GeneratedOptionsSmokeTestException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeneratedOptionsSmokeTestException"/> class.
    /// </summary>
    /// <param name="optionsType">The options type under test.</param>
    /// <param name="propertyName">The property under test.</param>
    /// <param name="innerException">The rendering failure.</param>
    public GeneratedOptionsSmokeTestException(
        Type optionsType,
        string propertyName,
        Exception innerException)
        : base($"{optionsType.FullName}.{propertyName} failed generated-options smoke testing.", innerException)
    {
        OptionsType = optionsType;
        PropertyName = propertyName;
    }

    /// <summary>
    /// Gets the options type under test.
    /// </summary>
    public Type OptionsType { get; }

    /// <summary>
    /// Gets the property under test.
    /// </summary>
    public string PropertyName { get; }
}
