using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Snapshots;

public class GeneratedCliRenderingSnapshotTests
{
    private const string SnapshotFileName = "GeneratedCliRendering.snap.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };

    [Test]
    public async Task EveryGeneratedOptionsClass_MatchesRenderingSnapshot()
    {
        var repositoryRoot = FindRepositoryRoot();
        var actual = CreateSnapshots(repositoryRoot);
        var actualJson = Serialize(actual);
        var snapshotPath = Path.Combine(repositoryRoot, "test", "ModularPipelines.UnitTests", "Snapshots", SnapshotFileName);

        if (string.Equals(
                Environment.GetEnvironmentVariable("UPDATE_CLI_RENDERING_SNAPSHOTS"),
                "true",
                StringComparison.OrdinalIgnoreCase))
        {
            await File.WriteAllTextAsync(snapshotPath, actualJson);
            return;
        }

        var expectedJson = (await File.ReadAllTextAsync(snapshotPath)).ReplaceLineEndings("\n");
        if (!string.Equals(expectedJson, actualJson, StringComparison.Ordinal))
        {
            await File.WriteAllTextAsync(snapshotPath + ".received.json", actualJson);
        }

        await Assert.That(actualJson).IsEqualTo(expectedJson)
            .Because($"Generated CLI rendering changed. Inspect {SnapshotFileName}.received.json and update the snapshot intentionally.");
    }

    private static SortedDictionary<string, PackageSnapshot> CreateSnapshots(string repositoryRoot)
    {
        var snapshots = new SortedDictionary<string, PackageSnapshot>(StringComparer.Ordinal);

        foreach (var packageName in GetGeneratedPackageNames(repositoryRoot))
        {
            var assembly = System.Reflection.Assembly.Load(packageName);
            snapshots.Add(packageName, CreatePackageSnapshot(assembly));
        }

        return snapshots;
    }

    private static PackageSnapshot CreatePackageSnapshot(System.Reflection.Assembly assembly)
    {
        var modelProvider = new DeterministicCommandModelProvider();
        var commandBuilder = new CommandLineBuilder(
            new ToolResolver(),
            new CommandPartsProvider(),
            new PlaceholderHandler(),
            modelProvider,
            new CommandArgumentBuilder());
        var corpus = new StringBuilder();
        var propertyCount = 0;
        var secretPropertyCount = 0;
        var renderedArgumentCount = 0;
        var typeHashes = new SortedDictionary<string, string>(StringComparer.Ordinal);
        var originalCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var optionTypes = assembly.GetTypes()
                .Where(IsGeneratedOptionsType)
                .OrderBy(x => x.FullName, StringComparer.Ordinal)
                .ToArray();

            foreach (var optionType in optionTypes)
            {
                var options = (CommandLineToolOptions) RuntimeHelpers.GetUninitializedObject(optionType);
                var commandModel = modelProvider.GetCommandModel(optionType);
                var typeCorpus = new StringBuilder();

                foreach (var part in commandModel)
                {
                    var isSecret = part.Property.IsDefined(typeof(SecretValueAttribute), inherit: true);
                    part.Property.SetValue(options, CreateRepresentativeValue(part.Property.PropertyType, isSecret));

                    propertyCount++;
                    secretPropertyCount += isSecret ? 1 : 0;
                }

                var commandLine = commandBuilder.Build(options);
                renderedArgumentCount += commandLine.Arguments.Count;

                typeCorpus.Append(optionType.FullName).Append('|');
                foreach (var part in commandModel)
                {
                    typeCorpus.Append(part.GetType().Name)
                        .Append(':')
                        .Append(part.Property.Name)
                        .Append(':')
                        .Append(part.Property.PropertyType.FullName)
                        .Append(':')
                        .Append(part.Property.IsDefined(typeof(SecretValueAttribute), inherit: true) ? "secret" : "plain")
                        .Append(';');
                }

                typeCorpus.Append('|').Append(commandLine).Append('\n');
                var serializedType = typeCorpus.ToString();
                corpus.Append(serializedType);
                typeHashes.Add(optionType.FullName!, Hash(serializedType));
            }

            return new PackageSnapshot(
                optionTypes.Length,
                propertyCount,
                secretPropertyCount,
                renderedArgumentCount,
                Hash(corpus.ToString()),
                typeHashes);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    private static bool IsGeneratedOptionsType(Type type)
    {
        return !type.IsAbstract
               && type.IsAssignableTo(typeof(CommandLineToolOptions))
               && type.GetCustomAttribute<GeneratedCodeAttribute>()?.Tool == "ModularPipelines.OptionsGenerator";
    }

    private static string Hash(string value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();

    private static object CreateRepresentativeValue(Type propertyType, bool isSecret)
    {
        var type = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (type == typeof(string))
        {
            return isSecret ? "secret-value" : "value";
        }

        if (type == typeof(bool))
        {
            return true;
        }

        if (type == typeof(int))
        {
            return 2;
        }

        if (type == typeof(double))
        {
            return 2d;
        }

        if (type == typeof(KeyValue))
        {
            return new KeyValue("key", isSecret ? "secret-value" : "value");
        }

        if (type.IsEnum)
        {
            return Enum.GetValues(type).GetValue(0)
                   ?? throw new InvalidOperationException($"Generated enum {type.FullName} has no values.");
        }

        if (type.IsArray)
        {
            return CreateRepresentativeArray(type.GetElementType()!, isSecret);
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            return CreateRepresentativeArray(type.GetGenericArguments()[0], isSecret);
        }

        throw new NotSupportedException($"No representative CLI value is defined for {propertyType.FullName}.");
    }

    private static Array CreateRepresentativeArray(Type elementType, bool isSecret)
    {
        var values = Array.CreateInstance(elementType, 2);
        values.SetValue(CreateRepresentativeValue(elementType, isSecret), 0);
        values.SetValue(CreateRepresentativeValue(elementType, isSecret), 1);
        return values;
    }

    private static IEnumerable<string> GetGeneratedPackageNames(string repositoryRoot)
    {
        var sourceDirectory = Path.Combine(repositoryRoot, "src");

        return Directory.EnumerateDirectories(sourceDirectory, "ModularPipelines.*")
            .Where(packageDirectory => Directory.Exists(Path.Combine(packageDirectory, "Options")))
            .Where(packageDirectory => Directory.EnumerateFiles(
                Path.Combine(packageDirectory, "Options"),
                "*.Generated.cs",
                SearchOption.TopDirectoryOnly).Any())
            .Select(Path.GetFileName)
            .OfType<string>()
            .OrderBy(x => x, StringComparer.Ordinal);
    }

    private static string FindRepositoryRoot()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, "src"))
                && File.Exists(Path.Combine(directory.FullName, "global.json")))
            {
                return directory.FullName;
            }
        }

        throw new DirectoryNotFoundException("Could not locate the ModularPipelines repository root.");
    }

    private static string Serialize(SortedDictionary<string, PackageSnapshot> snapshots)
    {
        return JsonSerializer.Serialize(snapshots, JsonOptions).ReplaceLineEndings("\n") + "\n";
    }

    private sealed class DeterministicCommandModelProvider : ICommandModelProvider
    {
        private readonly CommandModelProvider _inner = new();

        public IReadOnlyList<PropertyCommandLinePart> GetCommandModel(Type optionsType) =>
            _inner.GetCommandModel(optionsType)
                .OrderBy(part => part.Property.Name, StringComparer.Ordinal)
                .ThenBy(part => part.Property.DeclaringType?.FullName, StringComparer.Ordinal)
                .ThenBy(part => part.GetType().Name, StringComparer.Ordinal)
                .ToArray();
    }

    private sealed record PackageSnapshot(
        int Options,
        int Properties,
        int SecretProperties,
        int RenderedArguments,
        string Sha256,
        SortedDictionary<string, string> TypeSha256);
}
