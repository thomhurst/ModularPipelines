namespace ModularPipelines.UnitTests.FSharp

/// <summary>
/// Tests for large-scale pipeline scenarios to validate scalability,
/// performance, and correct behavior with many modules.
/// </summary>
/// <remarks>
/// These tests verify that the pipeline engine handles large numbers of modules
/// correctly, including proper parallelization, dependency ordering, and resource management.
///
/// Note: ModularPipelines requires each module to be a unique type (by design,
/// dependencies are resolved by type). These tests use generic types with different
/// type parameters to create many unique module types at compile time.
/// </remarks>
[<TUnit.Core.NotInParallel>]
type ScaleTests() =
    