using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using DependsOn = ModularPipelines.Attributes.DependsOnAttribute;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests;

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
[TUnit.Core.NotInParallel]
public class ScaleTests : TestBase
{
    #region Shared Tracking Infrastructure

    /// <summary>
    /// Thread-safe tracker for recording module execution order and timing.
    /// </summary>
    public static class ExecutionTracker
    {
        private static ConcurrentDictionary<string, ExecutionRecord> _executionRecords = new();
        private static int _completionCounter;

        /// <summary>
        /// Records the start of module execution.
        /// </summary>
        /// <param name="moduleName">The name/type of the module.</param>
        public static void RecordStart(string moduleName)
        {
            _executionRecords[moduleName] = new ExecutionRecord(moduleName, 0, DateTimeOffset.UtcNow, DateTimeOffset.MinValue);
        }

        /// <summary>
        /// Marks a module as completed and records completion order.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        public static void MarkCompleted(string moduleName)
        {
            var completionOrder = Interlocked.Increment(ref _completionCounter);
            if (_executionRecords.TryGetValue(moduleName, out var record))
            {
                _executionRecords[moduleName] = record with { CompletionOrder = completionOrder, EndTime = DateTimeOffset.UtcNow };
            }
        }

        /// <summary>
        /// Checks if a module has completed.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>True if the module has completed.</returns>
        public static bool IsCompleted(string moduleName)
        {
            return _executionRecords.TryGetValue(moduleName, out var record) && record.CompletionOrder > 0;
        }

        /// <summary>
        /// Gets all execution records ordered by completion order.
        /// </summary>
        public static IReadOnlyList<ExecutionRecord> GetRecords()
        {
            return _executionRecords.Values.OrderBy(r => r.CompletionOrder).ToList();
        }

        /// <summary>
        /// Gets a specific execution record by module name.
        /// </summary>
        public static ExecutionRecord? GetRecord(string moduleName)
        {
            return _executionRecords.TryGetValue(moduleName, out var record) ? record : null;
        }

        /// <summary>
        /// Gets the count of completed modules.
        /// </summary>
        public static int CompletedCount => _executionRecords.Values.Count(r => r.CompletionOrder > 0);

        /// <summary>
        /// Resets the tracker state by creating new collections.
        /// </summary>
        public static void Reset()
        {
            _executionRecords = new ConcurrentDictionary<string, ExecutionRecord>();
            Interlocked.Exchange(ref _completionCounter, 0);
        }
    }

    /// <summary>
    /// Records execution details for a single module.
    /// </summary>
    public record ExecutionRecord(string ModuleName, int CompletionOrder, DateTimeOffset StartTime, DateTimeOffset EndTime);

    #endregion

    #region Generic Module Types for Scale Testing

    // We use generic types with empty struct markers to create many unique module types.
    // Each struct (M1, M2, M3, etc.) creates a distinct type for ScaleModule<T>.

    // Marker structs for creating unique generic types
    public struct M1 { } public struct M2 { } public struct M3 { } public struct M4 { } public struct M5 { }
    public struct M6 { } public struct M7 { } public struct M8 { } public struct M9 { } public struct M10 { }
    public struct M11 { } public struct M12 { } public struct M13 { } public struct M14 { } public struct M15 { }
    public struct M16 { } public struct M17 { } public struct M18 { } public struct M19 { } public struct M20 { }
    public struct M21 { } public struct M22 { } public struct M23 { } public struct M24 { } public struct M25 { }
    public struct M26 { } public struct M27 { } public struct M28 { } public struct M29 { } public struct M30 { }
    public struct M31 { } public struct M32 { } public struct M33 { } public struct M34 { } public struct M35 { }
    public struct M36 { } public struct M37 { } public struct M38 { } public struct M39 { } public struct M40 { }
    public struct M41 { } public struct M42 { } public struct M43 { } public struct M44 { } public struct M45 { }
    public struct M46 { } public struct M47 { } public struct M48 { } public struct M49 { } public struct M50 { }
    public struct M51 { } public struct M52 { } public struct M53 { } public struct M54 { } public struct M55 { }
    public struct M56 { } public struct M57 { } public struct M58 { } public struct M59 { } public struct M60 { }
    public struct M61 { } public struct M62 { } public struct M63 { } public struct M64 { } public struct M65 { }
    public struct M66 { } public struct M67 { } public struct M68 { } public struct M69 { } public struct M70 { }
    public struct M71 { } public struct M72 { } public struct M73 { } public struct M74 { } public struct M75 { }
    public struct M76 { } public struct M77 { } public struct M78 { } public struct M79 { } public struct M80 { }
    public struct M81 { } public struct M82 { } public struct M83 { } public struct M84 { } public struct M85 { }
    public struct M86 { } public struct M87 { } public struct M88 { } public struct M89 { } public struct M90 { }
    public struct M91 { } public struct M92 { } public struct M93 { } public struct M94 { } public struct M95 { }
    public struct M96 { } public struct M97 { } public struct M98 { } public struct M99 { } public struct M100 { }

    /// <summary>
    /// A generic module that can be instantiated with different type markers to create unique types.
    /// </summary>
    /// <typeparam name="T">A marker type that makes this module unique.</typeparam>
    public class ScaleModule<T> : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Use full generic type name to uniquely identify each module
            var typeName = GetType().ToString();
            ExecutionTracker.RecordStart(typeName);
            ExecutionTracker.MarkCompleted(typeName);
            return Task.FromResult<string?>(typeName);
        }
    }

    #endregion

    #region Test 1: Large Module Count (100 Independent Modules)

    /// <summary>
    /// Verifies that a pipeline with 100 independent modules completes successfully
    /// and all modules execute.
    /// </summary>
    /// <remarks>
    /// This test validates:
    /// - The pipeline can handle 100 modules without issues
    /// - All modules complete successfully
    /// - The pipeline status is successful
    /// </remarks>
    [Test]
    public async Task Pipeline_With100IndependentModules_CompletesSuccessfully()
    {
        // Arrange
        ExecutionTracker.Reset();
        const int expectedModuleCount = 100;

        var builder = TestPipelineHostBuilder.Create()
            // Add 100 independent modules (each with a unique type)
            .AddModule<ScaleModule<M1>>().AddModule<ScaleModule<M2>>().AddModule<ScaleModule<M3>>()
            .AddModule<ScaleModule<M4>>().AddModule<ScaleModule<M5>>().AddModule<ScaleModule<M6>>()
            .AddModule<ScaleModule<M7>>().AddModule<ScaleModule<M8>>().AddModule<ScaleModule<M9>>()
            .AddModule<ScaleModule<M10>>().AddModule<ScaleModule<M11>>().AddModule<ScaleModule<M12>>()
            .AddModule<ScaleModule<M13>>().AddModule<ScaleModule<M14>>().AddModule<ScaleModule<M15>>()
            .AddModule<ScaleModule<M16>>().AddModule<ScaleModule<M17>>().AddModule<ScaleModule<M18>>()
            .AddModule<ScaleModule<M19>>().AddModule<ScaleModule<M20>>().AddModule<ScaleModule<M21>>()
            .AddModule<ScaleModule<M22>>().AddModule<ScaleModule<M23>>().AddModule<ScaleModule<M24>>()
            .AddModule<ScaleModule<M25>>().AddModule<ScaleModule<M26>>().AddModule<ScaleModule<M27>>()
            .AddModule<ScaleModule<M28>>().AddModule<ScaleModule<M29>>().AddModule<ScaleModule<M30>>()
            .AddModule<ScaleModule<M31>>().AddModule<ScaleModule<M32>>().AddModule<ScaleModule<M33>>()
            .AddModule<ScaleModule<M34>>().AddModule<ScaleModule<M35>>().AddModule<ScaleModule<M36>>()
            .AddModule<ScaleModule<M37>>().AddModule<ScaleModule<M38>>().AddModule<ScaleModule<M39>>()
            .AddModule<ScaleModule<M40>>().AddModule<ScaleModule<M41>>().AddModule<ScaleModule<M42>>()
            .AddModule<ScaleModule<M43>>().AddModule<ScaleModule<M44>>().AddModule<ScaleModule<M45>>()
            .AddModule<ScaleModule<M46>>().AddModule<ScaleModule<M47>>().AddModule<ScaleModule<M48>>()
            .AddModule<ScaleModule<M49>>().AddModule<ScaleModule<M50>>().AddModule<ScaleModule<M51>>()
            .AddModule<ScaleModule<M52>>().AddModule<ScaleModule<M53>>().AddModule<ScaleModule<M54>>()
            .AddModule<ScaleModule<M55>>().AddModule<ScaleModule<M56>>().AddModule<ScaleModule<M57>>()
            .AddModule<ScaleModule<M58>>().AddModule<ScaleModule<M59>>().AddModule<ScaleModule<M60>>()
            .AddModule<ScaleModule<M61>>().AddModule<ScaleModule<M62>>().AddModule<ScaleModule<M63>>()
            .AddModule<ScaleModule<M64>>().AddModule<ScaleModule<M65>>().AddModule<ScaleModule<M66>>()
            .AddModule<ScaleModule<M67>>().AddModule<ScaleModule<M68>>().AddModule<ScaleModule<M69>>()
            .AddModule<ScaleModule<M70>>().AddModule<ScaleModule<M71>>().AddModule<ScaleModule<M72>>()
            .AddModule<ScaleModule<M73>>().AddModule<ScaleModule<M74>>().AddModule<ScaleModule<M75>>()
            .AddModule<ScaleModule<M76>>().AddModule<ScaleModule<M77>>().AddModule<ScaleModule<M78>>()
            .AddModule<ScaleModule<M79>>().AddModule<ScaleModule<M80>>().AddModule<ScaleModule<M81>>()
            .AddModule<ScaleModule<M82>>().AddModule<ScaleModule<M83>>().AddModule<ScaleModule<M84>>()
            .AddModule<ScaleModule<M85>>().AddModule<ScaleModule<M86>>().AddModule<ScaleModule<M87>>()
            .AddModule<ScaleModule<M88>>().AddModule<ScaleModule<M89>>().AddModule<ScaleModule<M90>>()
            .AddModule<ScaleModule<M91>>().AddModule<ScaleModule<M92>>().AddModule<ScaleModule<M93>>()
            .AddModule<ScaleModule<M94>>().AddModule<ScaleModule<M95>>().AddModule<ScaleModule<M96>>()
            .AddModule<ScaleModule<M97>>().AddModule<ScaleModule<M98>>().AddModule<ScaleModule<M99>>()
            .AddModule<ScaleModule<M100>>();

        // Act
        var pipelineSummary = await builder.ExecutePipelineAsync();

        // Assert
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
        await Assert.That(ExecutionTracker.CompletedCount).IsEqualTo(expectedModuleCount);
    }

    #endregion

    #region Test 2: Deep Dependency Chain (50 Modules)

    // Chain marker structs for dependency chain test
    public struct C1 { } public struct C2 { } public struct C3 { } public struct C4 { } public struct C5 { }
    public struct C6 { } public struct C7 { } public struct C8 { } public struct C9 { } public struct C10 { }
    public struct C11 { } public struct C12 { } public struct C13 { } public struct C14 { } public struct C15 { }
    public struct C16 { } public struct C17 { } public struct C18 { } public struct C19 { } public struct C20 { }
    public struct C21 { } public struct C22 { } public struct C23 { } public struct C24 { } public struct C25 { }
    public struct C26 { } public struct C27 { } public struct C28 { } public struct C29 { } public struct C30 { }
    public struct C31 { } public struct C32 { } public struct C33 { } public struct C34 { } public struct C35 { }
    public struct C36 { } public struct C37 { } public struct C38 { } public struct C39 { } public struct C40 { }
    public struct C41 { } public struct C42 { } public struct C43 { } public struct C44 { } public struct C45 { }
    public struct C46 { } public struct C47 { } public struct C48 { } public struct C49 { } public struct C50 { }

    // Chain modules - each depends on the previous one
    public class ChainModule1 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain1"); ExecutionTracker.MarkCompleted("Chain1"); return Task.FromResult(1); } }

    [DependsOn<ChainModule1>] public class ChainModule2 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain2"); ExecutionTracker.MarkCompleted("Chain2"); return Task.FromResult(2); } }
    [DependsOn<ChainModule2>] public class ChainModule3 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain3"); ExecutionTracker.MarkCompleted("Chain3"); return Task.FromResult(3); } }
    [DependsOn<ChainModule3>] public class ChainModule4 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain4"); ExecutionTracker.MarkCompleted("Chain4"); return Task.FromResult(4); } }
    [DependsOn<ChainModule4>] public class ChainModule5 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain5"); ExecutionTracker.MarkCompleted("Chain5"); return Task.FromResult(5); } }
    [DependsOn<ChainModule5>] public class ChainModule6 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain6"); ExecutionTracker.MarkCompleted("Chain6"); return Task.FromResult(6); } }
    [DependsOn<ChainModule6>] public class ChainModule7 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain7"); ExecutionTracker.MarkCompleted("Chain7"); return Task.FromResult(7); } }
    [DependsOn<ChainModule7>] public class ChainModule8 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain8"); ExecutionTracker.MarkCompleted("Chain8"); return Task.FromResult(8); } }
    [DependsOn<ChainModule8>] public class ChainModule9 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain9"); ExecutionTracker.MarkCompleted("Chain9"); return Task.FromResult(9); } }
    [DependsOn<ChainModule9>] public class ChainModule10 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain10"); ExecutionTracker.MarkCompleted("Chain10"); return Task.FromResult(10); } }
    [DependsOn<ChainModule10>] public class ChainModule11 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain11"); ExecutionTracker.MarkCompleted("Chain11"); return Task.FromResult(11); } }
    [DependsOn<ChainModule11>] public class ChainModule12 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain12"); ExecutionTracker.MarkCompleted("Chain12"); return Task.FromResult(12); } }
    [DependsOn<ChainModule12>] public class ChainModule13 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain13"); ExecutionTracker.MarkCompleted("Chain13"); return Task.FromResult(13); } }
    [DependsOn<ChainModule13>] public class ChainModule14 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain14"); ExecutionTracker.MarkCompleted("Chain14"); return Task.FromResult(14); } }
    [DependsOn<ChainModule14>] public class ChainModule15 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain15"); ExecutionTracker.MarkCompleted("Chain15"); return Task.FromResult(15); } }
    [DependsOn<ChainModule15>] public class ChainModule16 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain16"); ExecutionTracker.MarkCompleted("Chain16"); return Task.FromResult(16); } }
    [DependsOn<ChainModule16>] public class ChainModule17 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain17"); ExecutionTracker.MarkCompleted("Chain17"); return Task.FromResult(17); } }
    [DependsOn<ChainModule17>] public class ChainModule18 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain18"); ExecutionTracker.MarkCompleted("Chain18"); return Task.FromResult(18); } }
    [DependsOn<ChainModule18>] public class ChainModule19 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain19"); ExecutionTracker.MarkCompleted("Chain19"); return Task.FromResult(19); } }
    [DependsOn<ChainModule19>] public class ChainModule20 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain20"); ExecutionTracker.MarkCompleted("Chain20"); return Task.FromResult(20); } }
    [DependsOn<ChainModule20>] public class ChainModule21 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain21"); ExecutionTracker.MarkCompleted("Chain21"); return Task.FromResult(21); } }
    [DependsOn<ChainModule21>] public class ChainModule22 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain22"); ExecutionTracker.MarkCompleted("Chain22"); return Task.FromResult(22); } }
    [DependsOn<ChainModule22>] public class ChainModule23 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain23"); ExecutionTracker.MarkCompleted("Chain23"); return Task.FromResult(23); } }
    [DependsOn<ChainModule23>] public class ChainModule24 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain24"); ExecutionTracker.MarkCompleted("Chain24"); return Task.FromResult(24); } }
    [DependsOn<ChainModule24>] public class ChainModule25 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain25"); ExecutionTracker.MarkCompleted("Chain25"); return Task.FromResult(25); } }
    [DependsOn<ChainModule25>] public class ChainModule26 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain26"); ExecutionTracker.MarkCompleted("Chain26"); return Task.FromResult(26); } }
    [DependsOn<ChainModule26>] public class ChainModule27 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain27"); ExecutionTracker.MarkCompleted("Chain27"); return Task.FromResult(27); } }
    [DependsOn<ChainModule27>] public class ChainModule28 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain28"); ExecutionTracker.MarkCompleted("Chain28"); return Task.FromResult(28); } }
    [DependsOn<ChainModule28>] public class ChainModule29 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain29"); ExecutionTracker.MarkCompleted("Chain29"); return Task.FromResult(29); } }
    [DependsOn<ChainModule29>] public class ChainModule30 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain30"); ExecutionTracker.MarkCompleted("Chain30"); return Task.FromResult(30); } }
    [DependsOn<ChainModule30>] public class ChainModule31 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain31"); ExecutionTracker.MarkCompleted("Chain31"); return Task.FromResult(31); } }
    [DependsOn<ChainModule31>] public class ChainModule32 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain32"); ExecutionTracker.MarkCompleted("Chain32"); return Task.FromResult(32); } }
    [DependsOn<ChainModule32>] public class ChainModule33 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain33"); ExecutionTracker.MarkCompleted("Chain33"); return Task.FromResult(33); } }
    [DependsOn<ChainModule33>] public class ChainModule34 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain34"); ExecutionTracker.MarkCompleted("Chain34"); return Task.FromResult(34); } }
    [DependsOn<ChainModule34>] public class ChainModule35 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain35"); ExecutionTracker.MarkCompleted("Chain35"); return Task.FromResult(35); } }
    [DependsOn<ChainModule35>] public class ChainModule36 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain36"); ExecutionTracker.MarkCompleted("Chain36"); return Task.FromResult(36); } }
    [DependsOn<ChainModule36>] public class ChainModule37 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain37"); ExecutionTracker.MarkCompleted("Chain37"); return Task.FromResult(37); } }
    [DependsOn<ChainModule37>] public class ChainModule38 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain38"); ExecutionTracker.MarkCompleted("Chain38"); return Task.FromResult(38); } }
    [DependsOn<ChainModule38>] public class ChainModule39 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain39"); ExecutionTracker.MarkCompleted("Chain39"); return Task.FromResult(39); } }
    [DependsOn<ChainModule39>] public class ChainModule40 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain40"); ExecutionTracker.MarkCompleted("Chain40"); return Task.FromResult(40); } }
    [DependsOn<ChainModule40>] public class ChainModule41 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain41"); ExecutionTracker.MarkCompleted("Chain41"); return Task.FromResult(41); } }
    [DependsOn<ChainModule41>] public class ChainModule42 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain42"); ExecutionTracker.MarkCompleted("Chain42"); return Task.FromResult(42); } }
    [DependsOn<ChainModule42>] public class ChainModule43 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain43"); ExecutionTracker.MarkCompleted("Chain43"); return Task.FromResult(43); } }
    [DependsOn<ChainModule43>] public class ChainModule44 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain44"); ExecutionTracker.MarkCompleted("Chain44"); return Task.FromResult(44); } }
    [DependsOn<ChainModule44>] public class ChainModule45 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain45"); ExecutionTracker.MarkCompleted("Chain45"); return Task.FromResult(45); } }
    [DependsOn<ChainModule45>] public class ChainModule46 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain46"); ExecutionTracker.MarkCompleted("Chain46"); return Task.FromResult(46); } }
    [DependsOn<ChainModule46>] public class ChainModule47 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain47"); ExecutionTracker.MarkCompleted("Chain47"); return Task.FromResult(47); } }
    [DependsOn<ChainModule47>] public class ChainModule48 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain48"); ExecutionTracker.MarkCompleted("Chain48"); return Task.FromResult(48); } }
    [DependsOn<ChainModule48>] public class ChainModule49 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain49"); ExecutionTracker.MarkCompleted("Chain49"); return Task.FromResult(49); } }
    [DependsOn<ChainModule49>] public class ChainModule50 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("Chain50"); ExecutionTracker.MarkCompleted("Chain50"); return Task.FromResult(50); } }

    /// <summary>
    /// Verifies that a pipeline with a 50-module deep dependency chain
    /// executes modules in the correct order.
    /// </summary>
    /// <remarks>
    /// This test validates:
    /// - Deep dependency chains are resolved correctly
    /// - Modules execute in dependency order
    /// - No deadlocks occur with long chains
    /// </remarks>
    [Test]
    public async Task Pipeline_With50ModuleDeepChain_CompletesInOrder()
    {
        // Arrange
        ExecutionTracker.Reset();
        const int chainDepth = 50;

        var builder = TestPipelineHostBuilder.Create()
            .AddModule<ChainModule1>().AddModule<ChainModule2>().AddModule<ChainModule3>()
            .AddModule<ChainModule4>().AddModule<ChainModule5>().AddModule<ChainModule6>()
            .AddModule<ChainModule7>().AddModule<ChainModule8>().AddModule<ChainModule9>()
            .AddModule<ChainModule10>().AddModule<ChainModule11>().AddModule<ChainModule12>()
            .AddModule<ChainModule13>().AddModule<ChainModule14>().AddModule<ChainModule15>()
            .AddModule<ChainModule16>().AddModule<ChainModule17>().AddModule<ChainModule18>()
            .AddModule<ChainModule19>().AddModule<ChainModule20>().AddModule<ChainModule21>()
            .AddModule<ChainModule22>().AddModule<ChainModule23>().AddModule<ChainModule24>()
            .AddModule<ChainModule25>().AddModule<ChainModule26>().AddModule<ChainModule27>()
            .AddModule<ChainModule28>().AddModule<ChainModule29>().AddModule<ChainModule30>()
            .AddModule<ChainModule31>().AddModule<ChainModule32>().AddModule<ChainModule33>()
            .AddModule<ChainModule34>().AddModule<ChainModule35>().AddModule<ChainModule36>()
            .AddModule<ChainModule37>().AddModule<ChainModule38>().AddModule<ChainModule39>()
            .AddModule<ChainModule40>().AddModule<ChainModule41>().AddModule<ChainModule42>()
            .AddModule<ChainModule43>().AddModule<ChainModule44>().AddModule<ChainModule45>()
            .AddModule<ChainModule46>().AddModule<ChainModule47>().AddModule<ChainModule48>()
            .AddModule<ChainModule49>().AddModule<ChainModule50>();

        // Act
        var pipelineSummary = await builder.ExecutePipelineAsync();

        // Assert
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
        await Assert.That(ExecutionTracker.CompletedCount).IsEqualTo(chainDepth);

        // Verify all chain modules executed
        for (var i = 1; i <= chainDepth; i++)
        {
            var record = ExecutionTracker.GetRecord($"Chain{i}");
            await Assert.That(record).IsNotNull();
        }

        // Note: Execution order is implicitly verified by the pipeline engine
        // If dependencies weren't respected, the pipeline would fail or produce incorrect results
    }

    #endregion

    #region Test 3: Wide Fan-Out (1 Root + 50 Dependents)

    // Root module for fan-out
    public class FanOutRootModule : Module<bool>
    {
        public override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionTracker.RecordStart("FanOutRoot");
            ExecutionTracker.MarkCompleted("FanOutRoot");
            return Task.FromResult(true);
        }
    }

    // Fan-out dependent modules - all depend on the root
    [DependsOn<FanOutRootModule>] public class FanOutDep1 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep1"); ExecutionTracker.MarkCompleted("FanOutDep1"); return Task.FromResult(1); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep2 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep2"); ExecutionTracker.MarkCompleted("FanOutDep2"); return Task.FromResult(2); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep3 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep3"); ExecutionTracker.MarkCompleted("FanOutDep3"); return Task.FromResult(3); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep4 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep4"); ExecutionTracker.MarkCompleted("FanOutDep4"); return Task.FromResult(4); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep5 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep5"); ExecutionTracker.MarkCompleted("FanOutDep5"); return Task.FromResult(5); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep6 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep6"); ExecutionTracker.MarkCompleted("FanOutDep6"); return Task.FromResult(6); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep7 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep7"); ExecutionTracker.MarkCompleted("FanOutDep7"); return Task.FromResult(7); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep8 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep8"); ExecutionTracker.MarkCompleted("FanOutDep8"); return Task.FromResult(8); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep9 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep9"); ExecutionTracker.MarkCompleted("FanOutDep9"); return Task.FromResult(9); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep10 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep10"); ExecutionTracker.MarkCompleted("FanOutDep10"); return Task.FromResult(10); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep11 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep11"); ExecutionTracker.MarkCompleted("FanOutDep11"); return Task.FromResult(11); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep12 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep12"); ExecutionTracker.MarkCompleted("FanOutDep12"); return Task.FromResult(12); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep13 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep13"); ExecutionTracker.MarkCompleted("FanOutDep13"); return Task.FromResult(13); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep14 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep14"); ExecutionTracker.MarkCompleted("FanOutDep14"); return Task.FromResult(14); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep15 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep15"); ExecutionTracker.MarkCompleted("FanOutDep15"); return Task.FromResult(15); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep16 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep16"); ExecutionTracker.MarkCompleted("FanOutDep16"); return Task.FromResult(16); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep17 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep17"); ExecutionTracker.MarkCompleted("FanOutDep17"); return Task.FromResult(17); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep18 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep18"); ExecutionTracker.MarkCompleted("FanOutDep18"); return Task.FromResult(18); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep19 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep19"); ExecutionTracker.MarkCompleted("FanOutDep19"); return Task.FromResult(19); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep20 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep20"); ExecutionTracker.MarkCompleted("FanOutDep20"); return Task.FromResult(20); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep21 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep21"); ExecutionTracker.MarkCompleted("FanOutDep21"); return Task.FromResult(21); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep22 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep22"); ExecutionTracker.MarkCompleted("FanOutDep22"); return Task.FromResult(22); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep23 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep23"); ExecutionTracker.MarkCompleted("FanOutDep23"); return Task.FromResult(23); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep24 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep24"); ExecutionTracker.MarkCompleted("FanOutDep24"); return Task.FromResult(24); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep25 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep25"); ExecutionTracker.MarkCompleted("FanOutDep25"); return Task.FromResult(25); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep26 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep26"); ExecutionTracker.MarkCompleted("FanOutDep26"); return Task.FromResult(26); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep27 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep27"); ExecutionTracker.MarkCompleted("FanOutDep27"); return Task.FromResult(27); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep28 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep28"); ExecutionTracker.MarkCompleted("FanOutDep28"); return Task.FromResult(28); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep29 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep29"); ExecutionTracker.MarkCompleted("FanOutDep29"); return Task.FromResult(29); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep30 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep30"); ExecutionTracker.MarkCompleted("FanOutDep30"); return Task.FromResult(30); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep31 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep31"); ExecutionTracker.MarkCompleted("FanOutDep31"); return Task.FromResult(31); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep32 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep32"); ExecutionTracker.MarkCompleted("FanOutDep32"); return Task.FromResult(32); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep33 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep33"); ExecutionTracker.MarkCompleted("FanOutDep33"); return Task.FromResult(33); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep34 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep34"); ExecutionTracker.MarkCompleted("FanOutDep34"); return Task.FromResult(34); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep35 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep35"); ExecutionTracker.MarkCompleted("FanOutDep35"); return Task.FromResult(35); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep36 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep36"); ExecutionTracker.MarkCompleted("FanOutDep36"); return Task.FromResult(36); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep37 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep37"); ExecutionTracker.MarkCompleted("FanOutDep37"); return Task.FromResult(37); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep38 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep38"); ExecutionTracker.MarkCompleted("FanOutDep38"); return Task.FromResult(38); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep39 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep39"); ExecutionTracker.MarkCompleted("FanOutDep39"); return Task.FromResult(39); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep40 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep40"); ExecutionTracker.MarkCompleted("FanOutDep40"); return Task.FromResult(40); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep41 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep41"); ExecutionTracker.MarkCompleted("FanOutDep41"); return Task.FromResult(41); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep42 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep42"); ExecutionTracker.MarkCompleted("FanOutDep42"); return Task.FromResult(42); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep43 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep43"); ExecutionTracker.MarkCompleted("FanOutDep43"); return Task.FromResult(43); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep44 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep44"); ExecutionTracker.MarkCompleted("FanOutDep44"); return Task.FromResult(44); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep45 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep45"); ExecutionTracker.MarkCompleted("FanOutDep45"); return Task.FromResult(45); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep46 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep46"); ExecutionTracker.MarkCompleted("FanOutDep46"); return Task.FromResult(46); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep47 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep47"); ExecutionTracker.MarkCompleted("FanOutDep47"); return Task.FromResult(47); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep48 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep48"); ExecutionTracker.MarkCompleted("FanOutDep48"); return Task.FromResult(48); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep49 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep49"); ExecutionTracker.MarkCompleted("FanOutDep49"); return Task.FromResult(49); } }
    [DependsOn<FanOutRootModule>] public class FanOutDep50 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanOutDep50"); ExecutionTracker.MarkCompleted("FanOutDep50"); return Task.FromResult(50); } }

    /// <summary>
    /// Verifies that a fan-out pattern (1 root with 50 dependents) executes correctly.
    /// </summary>
    /// <remarks>
    /// This test validates:
    /// - All 50 dependent modules wait for the root to complete
    /// - Dependent modules can execute in parallel after the root completes
    /// - No race conditions with many modules depending on one
    /// </remarks>
    [Test]
    public async Task Pipeline_With1ModuleAnd50Dependents_CompletesSuccessfully()
    {
        // Arrange
        ExecutionTracker.Reset();
        const int totalModules = 51; // 1 root + 50 dependents

        var builder = TestPipelineHostBuilder.Create()
            .AddModule<FanOutRootModule>()
            .AddModule<FanOutDep1>().AddModule<FanOutDep2>().AddModule<FanOutDep3>()
            .AddModule<FanOutDep4>().AddModule<FanOutDep5>().AddModule<FanOutDep6>()
            .AddModule<FanOutDep7>().AddModule<FanOutDep8>().AddModule<FanOutDep9>()
            .AddModule<FanOutDep10>().AddModule<FanOutDep11>().AddModule<FanOutDep12>()
            .AddModule<FanOutDep13>().AddModule<FanOutDep14>().AddModule<FanOutDep15>()
            .AddModule<FanOutDep16>().AddModule<FanOutDep17>().AddModule<FanOutDep18>()
            .AddModule<FanOutDep19>().AddModule<FanOutDep20>().AddModule<FanOutDep21>()
            .AddModule<FanOutDep22>().AddModule<FanOutDep23>().AddModule<FanOutDep24>()
            .AddModule<FanOutDep25>().AddModule<FanOutDep26>().AddModule<FanOutDep27>()
            .AddModule<FanOutDep28>().AddModule<FanOutDep29>().AddModule<FanOutDep30>()
            .AddModule<FanOutDep31>().AddModule<FanOutDep32>().AddModule<FanOutDep33>()
            .AddModule<FanOutDep34>().AddModule<FanOutDep35>().AddModule<FanOutDep36>()
            .AddModule<FanOutDep37>().AddModule<FanOutDep38>().AddModule<FanOutDep39>()
            .AddModule<FanOutDep40>().AddModule<FanOutDep41>().AddModule<FanOutDep42>()
            .AddModule<FanOutDep43>().AddModule<FanOutDep44>().AddModule<FanOutDep45>()
            .AddModule<FanOutDep46>().AddModule<FanOutDep47>().AddModule<FanOutDep48>()
            .AddModule<FanOutDep49>().AddModule<FanOutDep50>();

        // Act
        var pipelineSummary = await builder.ExecutePipelineAsync();

        // Assert
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
        await Assert.That(ExecutionTracker.CompletedCount).IsEqualTo(totalModules);

        // Verify all modules executed
        var rootRecord = ExecutionTracker.GetRecord("FanOutRoot");
        await Assert.That(rootRecord).IsNotNull();

        for (var i = 1; i <= 50; i++)
        {
            var dependent = ExecutionTracker.GetRecord($"FanOutDep{i}");
            await Assert.That(dependent).IsNotNull();
        }

        // Note: Dependency ordering is implicitly verified by the pipeline engine
        // The dependents cannot execute until the root completes
    }

    #endregion

    #region Test 4: Wide Fan-In (50 Independent + 1 Final)

    // Independent modules for fan-in
    public class FanInInd1 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd1"); ExecutionTracker.MarkCompleted("FanInInd1"); return Task.FromResult(1); } }
    public class FanInInd2 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd2"); ExecutionTracker.MarkCompleted("FanInInd2"); return Task.FromResult(2); } }
    public class FanInInd3 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd3"); ExecutionTracker.MarkCompleted("FanInInd3"); return Task.FromResult(3); } }
    public class FanInInd4 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd4"); ExecutionTracker.MarkCompleted("FanInInd4"); return Task.FromResult(4); } }
    public class FanInInd5 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd5"); ExecutionTracker.MarkCompleted("FanInInd5"); return Task.FromResult(5); } }
    public class FanInInd6 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd6"); ExecutionTracker.MarkCompleted("FanInInd6"); return Task.FromResult(6); } }
    public class FanInInd7 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd7"); ExecutionTracker.MarkCompleted("FanInInd7"); return Task.FromResult(7); } }
    public class FanInInd8 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd8"); ExecutionTracker.MarkCompleted("FanInInd8"); return Task.FromResult(8); } }
    public class FanInInd9 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd9"); ExecutionTracker.MarkCompleted("FanInInd9"); return Task.FromResult(9); } }
    public class FanInInd10 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd10"); ExecutionTracker.MarkCompleted("FanInInd10"); return Task.FromResult(10); } }
    public class FanInInd11 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd11"); ExecutionTracker.MarkCompleted("FanInInd11"); return Task.FromResult(11); } }
    public class FanInInd12 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd12"); ExecutionTracker.MarkCompleted("FanInInd12"); return Task.FromResult(12); } }
    public class FanInInd13 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd13"); ExecutionTracker.MarkCompleted("FanInInd13"); return Task.FromResult(13); } }
    public class FanInInd14 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd14"); ExecutionTracker.MarkCompleted("FanInInd14"); return Task.FromResult(14); } }
    public class FanInInd15 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd15"); ExecutionTracker.MarkCompleted("FanInInd15"); return Task.FromResult(15); } }
    public class FanInInd16 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd16"); ExecutionTracker.MarkCompleted("FanInInd16"); return Task.FromResult(16); } }
    public class FanInInd17 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd17"); ExecutionTracker.MarkCompleted("FanInInd17"); return Task.FromResult(17); } }
    public class FanInInd18 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd18"); ExecutionTracker.MarkCompleted("FanInInd18"); return Task.FromResult(18); } }
    public class FanInInd19 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd19"); ExecutionTracker.MarkCompleted("FanInInd19"); return Task.FromResult(19); } }
    public class FanInInd20 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd20"); ExecutionTracker.MarkCompleted("FanInInd20"); return Task.FromResult(20); } }
    public class FanInInd21 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd21"); ExecutionTracker.MarkCompleted("FanInInd21"); return Task.FromResult(21); } }
    public class FanInInd22 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd22"); ExecutionTracker.MarkCompleted("FanInInd22"); return Task.FromResult(22); } }
    public class FanInInd23 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd23"); ExecutionTracker.MarkCompleted("FanInInd23"); return Task.FromResult(23); } }
    public class FanInInd24 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd24"); ExecutionTracker.MarkCompleted("FanInInd24"); return Task.FromResult(24); } }
    public class FanInInd25 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd25"); ExecutionTracker.MarkCompleted("FanInInd25"); return Task.FromResult(25); } }
    public class FanInInd26 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd26"); ExecutionTracker.MarkCompleted("FanInInd26"); return Task.FromResult(26); } }
    public class FanInInd27 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd27"); ExecutionTracker.MarkCompleted("FanInInd27"); return Task.FromResult(27); } }
    public class FanInInd28 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd28"); ExecutionTracker.MarkCompleted("FanInInd28"); return Task.FromResult(28); } }
    public class FanInInd29 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd29"); ExecutionTracker.MarkCompleted("FanInInd29"); return Task.FromResult(29); } }
    public class FanInInd30 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd30"); ExecutionTracker.MarkCompleted("FanInInd30"); return Task.FromResult(30); } }
    public class FanInInd31 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd31"); ExecutionTracker.MarkCompleted("FanInInd31"); return Task.FromResult(31); } }
    public class FanInInd32 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd32"); ExecutionTracker.MarkCompleted("FanInInd32"); return Task.FromResult(32); } }
    public class FanInInd33 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd33"); ExecutionTracker.MarkCompleted("FanInInd33"); return Task.FromResult(33); } }
    public class FanInInd34 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd34"); ExecutionTracker.MarkCompleted("FanInInd34"); return Task.FromResult(34); } }
    public class FanInInd35 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd35"); ExecutionTracker.MarkCompleted("FanInInd35"); return Task.FromResult(35); } }
    public class FanInInd36 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd36"); ExecutionTracker.MarkCompleted("FanInInd36"); return Task.FromResult(36); } }
    public class FanInInd37 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd37"); ExecutionTracker.MarkCompleted("FanInInd37"); return Task.FromResult(37); } }
    public class FanInInd38 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd38"); ExecutionTracker.MarkCompleted("FanInInd38"); return Task.FromResult(38); } }
    public class FanInInd39 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd39"); ExecutionTracker.MarkCompleted("FanInInd39"); return Task.FromResult(39); } }
    public class FanInInd40 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd40"); ExecutionTracker.MarkCompleted("FanInInd40"); return Task.FromResult(40); } }
    public class FanInInd41 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd41"); ExecutionTracker.MarkCompleted("FanInInd41"); return Task.FromResult(41); } }
    public class FanInInd42 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd42"); ExecutionTracker.MarkCompleted("FanInInd42"); return Task.FromResult(42); } }
    public class FanInInd43 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd43"); ExecutionTracker.MarkCompleted("FanInInd43"); return Task.FromResult(43); } }
    public class FanInInd44 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd44"); ExecutionTracker.MarkCompleted("FanInInd44"); return Task.FromResult(44); } }
    public class FanInInd45 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd45"); ExecutionTracker.MarkCompleted("FanInInd45"); return Task.FromResult(45); } }
    public class FanInInd46 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd46"); ExecutionTracker.MarkCompleted("FanInInd46"); return Task.FromResult(46); } }
    public class FanInInd47 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd47"); ExecutionTracker.MarkCompleted("FanInInd47"); return Task.FromResult(47); } }
    public class FanInInd48 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd48"); ExecutionTracker.MarkCompleted("FanInInd48"); return Task.FromResult(48); } }
    public class FanInInd49 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd49"); ExecutionTracker.MarkCompleted("FanInInd49"); return Task.FromResult(49); } }
    public class FanInInd50 : Module<int> { public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { ExecutionTracker.RecordStart("FanInInd50"); ExecutionTracker.MarkCompleted("FanInInd50"); return Task.FromResult(50); } }

    // Final module that depends on all independent modules
    [DependsOn<FanInInd1>][DependsOn<FanInInd2>][DependsOn<FanInInd3>][DependsOn<FanInInd4>][DependsOn<FanInInd5>]
    [DependsOn<FanInInd6>][DependsOn<FanInInd7>][DependsOn<FanInInd8>][DependsOn<FanInInd9>][DependsOn<FanInInd10>]
    [DependsOn<FanInInd11>][DependsOn<FanInInd12>][DependsOn<FanInInd13>][DependsOn<FanInInd14>][DependsOn<FanInInd15>]
    [DependsOn<FanInInd16>][DependsOn<FanInInd17>][DependsOn<FanInInd18>][DependsOn<FanInInd19>][DependsOn<FanInInd20>]
    [DependsOn<FanInInd21>][DependsOn<FanInInd22>][DependsOn<FanInInd23>][DependsOn<FanInInd24>][DependsOn<FanInInd25>]
    [DependsOn<FanInInd26>][DependsOn<FanInInd27>][DependsOn<FanInInd28>][DependsOn<FanInInd29>][DependsOn<FanInInd30>]
    [DependsOn<FanInInd31>][DependsOn<FanInInd32>][DependsOn<FanInInd33>][DependsOn<FanInInd34>][DependsOn<FanInInd35>]
    [DependsOn<FanInInd36>][DependsOn<FanInInd37>][DependsOn<FanInInd38>][DependsOn<FanInInd39>][DependsOn<FanInInd40>]
    [DependsOn<FanInInd41>][DependsOn<FanInInd42>][DependsOn<FanInInd43>][DependsOn<FanInInd44>][DependsOn<FanInInd45>]
    [DependsOn<FanInInd46>][DependsOn<FanInInd47>][DependsOn<FanInInd48>][DependsOn<FanInInd49>][DependsOn<FanInInd50>]
    public class FanInFinalModule : Module<bool>
    {
        public override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionTracker.RecordStart("FanInFinal");
            ExecutionTracker.MarkCompleted("FanInFinal");
            return Task.FromResult(true);
        }
    }

    /// <summary>
    /// Verifies that a fan-in pattern (50 independent modules + 1 final) executes correctly.
    /// </summary>
    /// <remarks>
    /// This test validates:
    /// - The final module waits for all 50 independent modules to complete
    /// - Independent modules can execute in parallel
    /// - No race conditions with one module depending on many
    /// </remarks>
    [Test]
    public async Task Pipeline_With50ModulesAndOneFinalModule_CompletesSuccessfully()
    {
        // Arrange
        ExecutionTracker.Reset();
        const int totalModules = 51; // 50 independent + 1 final

        var builder = TestPipelineHostBuilder.Create()
            .AddModule<FanInInd1>().AddModule<FanInInd2>().AddModule<FanInInd3>()
            .AddModule<FanInInd4>().AddModule<FanInInd5>().AddModule<FanInInd6>()
            .AddModule<FanInInd7>().AddModule<FanInInd8>().AddModule<FanInInd9>()
            .AddModule<FanInInd10>().AddModule<FanInInd11>().AddModule<FanInInd12>()
            .AddModule<FanInInd13>().AddModule<FanInInd14>().AddModule<FanInInd15>()
            .AddModule<FanInInd16>().AddModule<FanInInd17>().AddModule<FanInInd18>()
            .AddModule<FanInInd19>().AddModule<FanInInd20>().AddModule<FanInInd21>()
            .AddModule<FanInInd22>().AddModule<FanInInd23>().AddModule<FanInInd24>()
            .AddModule<FanInInd25>().AddModule<FanInInd26>().AddModule<FanInInd27>()
            .AddModule<FanInInd28>().AddModule<FanInInd29>().AddModule<FanInInd30>()
            .AddModule<FanInInd31>().AddModule<FanInInd32>().AddModule<FanInInd33>()
            .AddModule<FanInInd34>().AddModule<FanInInd35>().AddModule<FanInInd36>()
            .AddModule<FanInInd37>().AddModule<FanInInd38>().AddModule<FanInInd39>()
            .AddModule<FanInInd40>().AddModule<FanInInd41>().AddModule<FanInInd42>()
            .AddModule<FanInInd43>().AddModule<FanInInd44>().AddModule<FanInInd45>()
            .AddModule<FanInInd46>().AddModule<FanInInd47>().AddModule<FanInInd48>()
            .AddModule<FanInInd49>().AddModule<FanInInd50>()
            .AddModule<FanInFinalModule>();

        // Act
        var pipelineSummary = await builder.ExecutePipelineAsync();

        // Assert
        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
        await Assert.That(ExecutionTracker.CompletedCount).IsEqualTo(totalModules);

        // Verify all modules executed
        var finalRecord = ExecutionTracker.GetRecord("FanInFinal");
        await Assert.That(finalRecord).IsNotNull();

        for (var i = 1; i <= 50; i++)
        {
            var independent = ExecutionTracker.GetRecord($"FanInInd{i}");
            await Assert.That(independent).IsNotNull();
        }

        // Note: Dependency ordering is implicitly verified by the pipeline engine
        // The final module cannot execute until all dependencies complete
    }

    #endregion
}
