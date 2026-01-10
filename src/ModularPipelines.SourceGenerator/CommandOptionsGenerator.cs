using Microsoft.CodeAnalysis;

namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Source generator that validates CommandLineToolOptions classes
/// and generates optimized Build() methods.
/// </summary>
[Generator]
public sealed class CommandOptionsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Phase 2.2: Add syntax provider for options classes
        // Phase 2.3: Add validation diagnostics
        // Phase 2.4: Add Build() method generation
    }
}
