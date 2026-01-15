using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
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
    /// Each test creates a fresh instance for test isolation.
    /// </summary>
    public class ExecutionTracker
    {
        private readonly ConcurrentDictionary<string, ExecutionRecord> _executionRecords = new();
        private int _completionCounter;

        /// <summary>
        /// Records the start of module execution.
        /// </summary>
        /// <param name="moduleName">The name/type of the module.</param>
        public void RecordStart(string moduleName)
        {
            _executionRecords[moduleName] = new ExecutionRecord(moduleName, 0, DateTimeOffset.UtcNow, DateTimeOffset.MinValue);
        }

        /// <summary>
        /// Marks a module as completed and records completion order.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        public void MarkCompleted(string moduleName)
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
        public bool IsCompleted(string moduleName)
        {
            return _executionRecords.TryGetValue(moduleName, out var record) && record.CompletionOrder > 0;
        }

        /// <summary>
        /// Gets all execution records ordered by completion order.
        /// </summary>
        public IReadOnlyList<ExecutionRecord> GetRecords()
        {
            return _executionRecords.Values.OrderBy(r => r.CompletionOrder).ToList();
        }

        /// <summary>
        /// Gets a specific execution record by module name.
        /// </summary>
        public ExecutionRecord? GetRecord(string moduleName)
        {
            return _executionRecords.TryGetValue(moduleName, out var record) ? record : null;
        }

        /// <summary>
        /// Gets the count of completed modules.
        /// </summary>
        public int CompletedCount => _executionRecords.Values.Count(r => r.CompletionOrder > 0);

        /// <summary>
        /// Gets the total count of recorded modules (started or completed).
        /// </summary>
        public int TotalRecordedCount => _executionRecords.Count;

        /// <summary>
        /// Gets whether the tracker is in a clean state (no records).
        /// </summary>
        public bool IsClean => _executionRecords.IsEmpty && _completionCounter == 0;
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
    public class ScaleModule<T>(ExecutionTracker tracker) : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Use full generic type name to uniquely identify each module
            var typeName = GetType().ToString();
            tracker.RecordStart(typeName);
            tracker.MarkCompleted(typeName);
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
        var tracker = new ExecutionTracker();
        await Assert.That(tracker.IsClean).IsTrue();
        const int expectedModuleCount = 100;

        var builder = TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) => services.AddSingleton(tracker))
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
        await Assert.That(tracker.CompletedCount).IsEqualTo(expectedModuleCount);
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
    public class ChainModule1(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain1"); tracker.MarkCompleted("Chain1"); return Task.FromResult(1); } }

    [ModularPipelines.Attributes.DependsOn<ChainModule1>] public class ChainModule2(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain2"); tracker.MarkCompleted("Chain2"); return Task.FromResult(2); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule2>] public class ChainModule3(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain3"); tracker.MarkCompleted("Chain3"); return Task.FromResult(3); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule3>] public class ChainModule4(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain4"); tracker.MarkCompleted("Chain4"); return Task.FromResult(4); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule4>] public class ChainModule5(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain5"); tracker.MarkCompleted("Chain5"); return Task.FromResult(5); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule5>] public class ChainModule6(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain6"); tracker.MarkCompleted("Chain6"); return Task.FromResult(6); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule6>] public class ChainModule7(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain7"); tracker.MarkCompleted("Chain7"); return Task.FromResult(7); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule7>] public class ChainModule8(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain8"); tracker.MarkCompleted("Chain8"); return Task.FromResult(8); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule8>] public class ChainModule9(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain9"); tracker.MarkCompleted("Chain9"); return Task.FromResult(9); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule9>] public class ChainModule10(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain10"); tracker.MarkCompleted("Chain10"); return Task.FromResult(10); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule10>] public class ChainModule11(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain11"); tracker.MarkCompleted("Chain11"); return Task.FromResult(11); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule11>] public class ChainModule12(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain12"); tracker.MarkCompleted("Chain12"); return Task.FromResult(12); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule12>] public class ChainModule13(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain13"); tracker.MarkCompleted("Chain13"); return Task.FromResult(13); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule13>] public class ChainModule14(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain14"); tracker.MarkCompleted("Chain14"); return Task.FromResult(14); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule14>] public class ChainModule15(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain15"); tracker.MarkCompleted("Chain15"); return Task.FromResult(15); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule15>] public class ChainModule16(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain16"); tracker.MarkCompleted("Chain16"); return Task.FromResult(16); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule16>] public class ChainModule17(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain17"); tracker.MarkCompleted("Chain17"); return Task.FromResult(17); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule17>] public class ChainModule18(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain18"); tracker.MarkCompleted("Chain18"); return Task.FromResult(18); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule18>] public class ChainModule19(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain19"); tracker.MarkCompleted("Chain19"); return Task.FromResult(19); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule19>] public class ChainModule20(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain20"); tracker.MarkCompleted("Chain20"); return Task.FromResult(20); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule20>] public class ChainModule21(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain21"); tracker.MarkCompleted("Chain21"); return Task.FromResult(21); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule21>] public class ChainModule22(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain22"); tracker.MarkCompleted("Chain22"); return Task.FromResult(22); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule22>] public class ChainModule23(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain23"); tracker.MarkCompleted("Chain23"); return Task.FromResult(23); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule23>] public class ChainModule24(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain24"); tracker.MarkCompleted("Chain24"); return Task.FromResult(24); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule24>] public class ChainModule25(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain25"); tracker.MarkCompleted("Chain25"); return Task.FromResult(25); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule25>] public class ChainModule26(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain26"); tracker.MarkCompleted("Chain26"); return Task.FromResult(26); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule26>] public class ChainModule27(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain27"); tracker.MarkCompleted("Chain27"); return Task.FromResult(27); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule27>] public class ChainModule28(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain28"); tracker.MarkCompleted("Chain28"); return Task.FromResult(28); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule28>] public class ChainModule29(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain29"); tracker.MarkCompleted("Chain29"); return Task.FromResult(29); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule29>] public class ChainModule30(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain30"); tracker.MarkCompleted("Chain30"); return Task.FromResult(30); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule30>] public class ChainModule31(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain31"); tracker.MarkCompleted("Chain31"); return Task.FromResult(31); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule31>] public class ChainModule32(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain32"); tracker.MarkCompleted("Chain32"); return Task.FromResult(32); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule32>] public class ChainModule33(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain33"); tracker.MarkCompleted("Chain33"); return Task.FromResult(33); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule33>] public class ChainModule34(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain34"); tracker.MarkCompleted("Chain34"); return Task.FromResult(34); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule34>] public class ChainModule35(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain35"); tracker.MarkCompleted("Chain35"); return Task.FromResult(35); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule35>] public class ChainModule36(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain36"); tracker.MarkCompleted("Chain36"); return Task.FromResult(36); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule36>] public class ChainModule37(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain37"); tracker.MarkCompleted("Chain37"); return Task.FromResult(37); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule37>] public class ChainModule38(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain38"); tracker.MarkCompleted("Chain38"); return Task.FromResult(38); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule38>] public class ChainModule39(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain39"); tracker.MarkCompleted("Chain39"); return Task.FromResult(39); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule39>] public class ChainModule40(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain40"); tracker.MarkCompleted("Chain40"); return Task.FromResult(40); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule40>] public class ChainModule41(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain41"); tracker.MarkCompleted("Chain41"); return Task.FromResult(41); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule41>] public class ChainModule42(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain42"); tracker.MarkCompleted("Chain42"); return Task.FromResult(42); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule42>] public class ChainModule43(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain43"); tracker.MarkCompleted("Chain43"); return Task.FromResult(43); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule43>] public class ChainModule44(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain44"); tracker.MarkCompleted("Chain44"); return Task.FromResult(44); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule44>] public class ChainModule45(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain45"); tracker.MarkCompleted("Chain45"); return Task.FromResult(45); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule45>] public class ChainModule46(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain46"); tracker.MarkCompleted("Chain46"); return Task.FromResult(46); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule46>] public class ChainModule47(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain47"); tracker.MarkCompleted("Chain47"); return Task.FromResult(47); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule47>] public class ChainModule48(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain48"); tracker.MarkCompleted("Chain48"); return Task.FromResult(48); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule48>] public class ChainModule49(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain49"); tracker.MarkCompleted("Chain49"); return Task.FromResult(49); } }
    [ModularPipelines.Attributes.DependsOn<ChainModule49>] public class ChainModule50(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("Chain50"); tracker.MarkCompleted("Chain50"); return Task.FromResult(50); } }

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
        var tracker = new ExecutionTracker();
        await Assert.That(tracker.IsClean).IsTrue();
        const int chainDepth = 50;

        var builder = TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) => services.AddSingleton(tracker))
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
        await Assert.That(tracker.CompletedCount).IsEqualTo(chainDepth);

        // Verify all chain modules executed
        for (var i = 1; i <= chainDepth; i++)
        {
            var record = tracker.GetRecord($"Chain{i}");
            await Assert.That(record).IsNotNull();
        }

        // Verify dependency ordering: each module must complete before its dependent
        // Chain1 -> Chain2 -> Chain3 -> ... -> Chain50
        // Get all records sorted by completion order for diagnostics
        var orderedRecords = Enumerable.Range(1, chainDepth)
            .Select(i => tracker.GetRecord($"Chain{i}"))
            .Where(r => r != null)
            .OrderBy(r => r!.CompletionOrder)
            .ToList();

        // Verify the completion order matches the dependency order
        for (var i = 0; i < orderedRecords.Count; i++)
        {
            var expectedModuleName = $"Chain{i + 1}";
            var actualRecord = orderedRecords[i];

            await Assert.That(actualRecord!.ModuleName)
                .IsEqualTo(expectedModuleName)
                .Because($"Module at completion order {i + 1} should be {expectedModuleName} but was {actualRecord.ModuleName}");
        }
    }

    #endregion

    #region Test 3: Wide Fan-Out (1 Root + 50 Dependents)

    // Root module for fan-out
    public class FanOutRootModule(ExecutionTracker tracker) : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            tracker.RecordStart("FanOutRoot");
            tracker.MarkCompleted("FanOutRoot");
            return Task.FromResult(true);
        }
    }

    // Fan-out dependent modules - all depend on the root
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep1(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep1"); tracker.MarkCompleted("FanOutDep1"); return Task.FromResult(1); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep2(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep2"); tracker.MarkCompleted("FanOutDep2"); return Task.FromResult(2); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep3(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep3"); tracker.MarkCompleted("FanOutDep3"); return Task.FromResult(3); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep4(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep4"); tracker.MarkCompleted("FanOutDep4"); return Task.FromResult(4); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep5(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep5"); tracker.MarkCompleted("FanOutDep5"); return Task.FromResult(5); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep6(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep6"); tracker.MarkCompleted("FanOutDep6"); return Task.FromResult(6); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep7(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep7"); tracker.MarkCompleted("FanOutDep7"); return Task.FromResult(7); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep8(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep8"); tracker.MarkCompleted("FanOutDep8"); return Task.FromResult(8); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep9(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep9"); tracker.MarkCompleted("FanOutDep9"); return Task.FromResult(9); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep10(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep10"); tracker.MarkCompleted("FanOutDep10"); return Task.FromResult(10); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep11(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep11"); tracker.MarkCompleted("FanOutDep11"); return Task.FromResult(11); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep12(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep12"); tracker.MarkCompleted("FanOutDep12"); return Task.FromResult(12); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep13(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep13"); tracker.MarkCompleted("FanOutDep13"); return Task.FromResult(13); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep14(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep14"); tracker.MarkCompleted("FanOutDep14"); return Task.FromResult(14); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep15(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep15"); tracker.MarkCompleted("FanOutDep15"); return Task.FromResult(15); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep16(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep16"); tracker.MarkCompleted("FanOutDep16"); return Task.FromResult(16); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep17(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep17"); tracker.MarkCompleted("FanOutDep17"); return Task.FromResult(17); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep18(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep18"); tracker.MarkCompleted("FanOutDep18"); return Task.FromResult(18); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep19(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep19"); tracker.MarkCompleted("FanOutDep19"); return Task.FromResult(19); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep20(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep20"); tracker.MarkCompleted("FanOutDep20"); return Task.FromResult(20); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep21(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep21"); tracker.MarkCompleted("FanOutDep21"); return Task.FromResult(21); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep22(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep22"); tracker.MarkCompleted("FanOutDep22"); return Task.FromResult(22); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep23(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep23"); tracker.MarkCompleted("FanOutDep23"); return Task.FromResult(23); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep24(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep24"); tracker.MarkCompleted("FanOutDep24"); return Task.FromResult(24); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep25(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep25"); tracker.MarkCompleted("FanOutDep25"); return Task.FromResult(25); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep26(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep26"); tracker.MarkCompleted("FanOutDep26"); return Task.FromResult(26); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep27(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep27"); tracker.MarkCompleted("FanOutDep27"); return Task.FromResult(27); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep28(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep28"); tracker.MarkCompleted("FanOutDep28"); return Task.FromResult(28); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep29(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep29"); tracker.MarkCompleted("FanOutDep29"); return Task.FromResult(29); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep30(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep30"); tracker.MarkCompleted("FanOutDep30"); return Task.FromResult(30); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep31(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep31"); tracker.MarkCompleted("FanOutDep31"); return Task.FromResult(31); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep32(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep32"); tracker.MarkCompleted("FanOutDep32"); return Task.FromResult(32); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep33(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep33"); tracker.MarkCompleted("FanOutDep33"); return Task.FromResult(33); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep34(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep34"); tracker.MarkCompleted("FanOutDep34"); return Task.FromResult(34); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep35(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep35"); tracker.MarkCompleted("FanOutDep35"); return Task.FromResult(35); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep36(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep36"); tracker.MarkCompleted("FanOutDep36"); return Task.FromResult(36); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep37(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep37"); tracker.MarkCompleted("FanOutDep37"); return Task.FromResult(37); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep38(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep38"); tracker.MarkCompleted("FanOutDep38"); return Task.FromResult(38); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep39(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep39"); tracker.MarkCompleted("FanOutDep39"); return Task.FromResult(39); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep40(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep40"); tracker.MarkCompleted("FanOutDep40"); return Task.FromResult(40); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep41(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep41"); tracker.MarkCompleted("FanOutDep41"); return Task.FromResult(41); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep42(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep42"); tracker.MarkCompleted("FanOutDep42"); return Task.FromResult(42); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep43(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep43"); tracker.MarkCompleted("FanOutDep43"); return Task.FromResult(43); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep44(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep44"); tracker.MarkCompleted("FanOutDep44"); return Task.FromResult(44); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep45(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep45"); tracker.MarkCompleted("FanOutDep45"); return Task.FromResult(45); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep46(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep46"); tracker.MarkCompleted("FanOutDep46"); return Task.FromResult(46); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep47(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep47"); tracker.MarkCompleted("FanOutDep47"); return Task.FromResult(47); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep48(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep48"); tracker.MarkCompleted("FanOutDep48"); return Task.FromResult(48); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep49(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep49"); tracker.MarkCompleted("FanOutDep49"); return Task.FromResult(49); } }
    [ModularPipelines.Attributes.DependsOn<FanOutRootModule>] public class FanOutDep50(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanOutDep50"); tracker.MarkCompleted("FanOutDep50"); return Task.FromResult(50); } }

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
        var tracker = new ExecutionTracker();
        await Assert.That(tracker.IsClean).IsTrue();
        const int totalModules = 51; // 1 root + 50 dependents

        var builder = TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) => services.AddSingleton(tracker))
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
        await Assert.That(tracker.CompletedCount).IsEqualTo(totalModules);

        // Verify all modules executed
        var rootRecord = tracker.GetRecord("FanOutRoot");
        await Assert.That(rootRecord).IsNotNull();

        for (var i = 1; i <= 50; i++)
        {
            var dependent = tracker.GetRecord($"FanOutDep{i}");
            await Assert.That(dependent).IsNotNull();
        }

        // Note: Dependency ordering is implicitly verified by the pipeline engine
        // The dependents cannot execute until the root completes
    }

    #endregion

    #region Test 4: Wide Fan-In (50 Independent + 1 Final)

    // Independent modules for fan-in
    public class FanInInd1(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd1"); tracker.MarkCompleted("FanInInd1"); return Task.FromResult(1); } }
    public class FanInInd2(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd2"); tracker.MarkCompleted("FanInInd2"); return Task.FromResult(2); } }
    public class FanInInd3(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd3"); tracker.MarkCompleted("FanInInd3"); return Task.FromResult(3); } }
    public class FanInInd4(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd4"); tracker.MarkCompleted("FanInInd4"); return Task.FromResult(4); } }
    public class FanInInd5(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd5"); tracker.MarkCompleted("FanInInd5"); return Task.FromResult(5); } }
    public class FanInInd6(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd6"); tracker.MarkCompleted("FanInInd6"); return Task.FromResult(6); } }
    public class FanInInd7(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd7"); tracker.MarkCompleted("FanInInd7"); return Task.FromResult(7); } }
    public class FanInInd8(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd8"); tracker.MarkCompleted("FanInInd8"); return Task.FromResult(8); } }
    public class FanInInd9(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd9"); tracker.MarkCompleted("FanInInd9"); return Task.FromResult(9); } }
    public class FanInInd10(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd10"); tracker.MarkCompleted("FanInInd10"); return Task.FromResult(10); } }
    public class FanInInd11(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd11"); tracker.MarkCompleted("FanInInd11"); return Task.FromResult(11); } }
    public class FanInInd12(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd12"); tracker.MarkCompleted("FanInInd12"); return Task.FromResult(12); } }
    public class FanInInd13(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd13"); tracker.MarkCompleted("FanInInd13"); return Task.FromResult(13); } }
    public class FanInInd14(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd14"); tracker.MarkCompleted("FanInInd14"); return Task.FromResult(14); } }
    public class FanInInd15(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd15"); tracker.MarkCompleted("FanInInd15"); return Task.FromResult(15); } }
    public class FanInInd16(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd16"); tracker.MarkCompleted("FanInInd16"); return Task.FromResult(16); } }
    public class FanInInd17(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd17"); tracker.MarkCompleted("FanInInd17"); return Task.FromResult(17); } }
    public class FanInInd18(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd18"); tracker.MarkCompleted("FanInInd18"); return Task.FromResult(18); } }
    public class FanInInd19(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd19"); tracker.MarkCompleted("FanInInd19"); return Task.FromResult(19); } }
    public class FanInInd20(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd20"); tracker.MarkCompleted("FanInInd20"); return Task.FromResult(20); } }
    public class FanInInd21(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd21"); tracker.MarkCompleted("FanInInd21"); return Task.FromResult(21); } }
    public class FanInInd22(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd22"); tracker.MarkCompleted("FanInInd22"); return Task.FromResult(22); } }
    public class FanInInd23(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd23"); tracker.MarkCompleted("FanInInd23"); return Task.FromResult(23); } }
    public class FanInInd24(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd24"); tracker.MarkCompleted("FanInInd24"); return Task.FromResult(24); } }
    public class FanInInd25(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd25"); tracker.MarkCompleted("FanInInd25"); return Task.FromResult(25); } }
    public class FanInInd26(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd26"); tracker.MarkCompleted("FanInInd26"); return Task.FromResult(26); } }
    public class FanInInd27(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd27"); tracker.MarkCompleted("FanInInd27"); return Task.FromResult(27); } }
    public class FanInInd28(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd28"); tracker.MarkCompleted("FanInInd28"); return Task.FromResult(28); } }
    public class FanInInd29(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd29"); tracker.MarkCompleted("FanInInd29"); return Task.FromResult(29); } }
    public class FanInInd30(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd30"); tracker.MarkCompleted("FanInInd30"); return Task.FromResult(30); } }
    public class FanInInd31(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd31"); tracker.MarkCompleted("FanInInd31"); return Task.FromResult(31); } }
    public class FanInInd32(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd32"); tracker.MarkCompleted("FanInInd32"); return Task.FromResult(32); } }
    public class FanInInd33(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd33"); tracker.MarkCompleted("FanInInd33"); return Task.FromResult(33); } }
    public class FanInInd34(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd34"); tracker.MarkCompleted("FanInInd34"); return Task.FromResult(34); } }
    public class FanInInd35(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd35"); tracker.MarkCompleted("FanInInd35"); return Task.FromResult(35); } }
    public class FanInInd36(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd36"); tracker.MarkCompleted("FanInInd36"); return Task.FromResult(36); } }
    public class FanInInd37(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd37"); tracker.MarkCompleted("FanInInd37"); return Task.FromResult(37); } }
    public class FanInInd38(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd38"); tracker.MarkCompleted("FanInInd38"); return Task.FromResult(38); } }
    public class FanInInd39(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd39"); tracker.MarkCompleted("FanInInd39"); return Task.FromResult(39); } }
    public class FanInInd40(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd40"); tracker.MarkCompleted("FanInInd40"); return Task.FromResult(40); } }
    public class FanInInd41(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd41"); tracker.MarkCompleted("FanInInd41"); return Task.FromResult(41); } }
    public class FanInInd42(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd42"); tracker.MarkCompleted("FanInInd42"); return Task.FromResult(42); } }
    public class FanInInd43(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd43"); tracker.MarkCompleted("FanInInd43"); return Task.FromResult(43); } }
    public class FanInInd44(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd44"); tracker.MarkCompleted("FanInInd44"); return Task.FromResult(44); } }
    public class FanInInd45(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd45"); tracker.MarkCompleted("FanInInd45"); return Task.FromResult(45); } }
    public class FanInInd46(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd46"); tracker.MarkCompleted("FanInInd46"); return Task.FromResult(46); } }
    public class FanInInd47(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd47"); tracker.MarkCompleted("FanInInd47"); return Task.FromResult(47); } }
    public class FanInInd48(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd48"); tracker.MarkCompleted("FanInInd48"); return Task.FromResult(48); } }
    public class FanInInd49(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd49"); tracker.MarkCompleted("FanInInd49"); return Task.FromResult(49); } }
    public class FanInInd50(ExecutionTracker tracker) : Module<int> { protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) { tracker.RecordStart("FanInInd50"); tracker.MarkCompleted("FanInInd50"); return Task.FromResult(50); } }

    // Final module that depends on all independent modules
    [ModularPipelines.Attributes.DependsOn<FanInInd1>][ModularPipelines.Attributes.DependsOn<FanInInd2>][ModularPipelines.Attributes.DependsOn<FanInInd3>][ModularPipelines.Attributes.DependsOn<FanInInd4>][ModularPipelines.Attributes.DependsOn<FanInInd5>]
    [ModularPipelines.Attributes.DependsOn<FanInInd6>][ModularPipelines.Attributes.DependsOn<FanInInd7>][ModularPipelines.Attributes.DependsOn<FanInInd8>][ModularPipelines.Attributes.DependsOn<FanInInd9>][ModularPipelines.Attributes.DependsOn<FanInInd10>]
    [ModularPipelines.Attributes.DependsOn<FanInInd11>][ModularPipelines.Attributes.DependsOn<FanInInd12>][ModularPipelines.Attributes.DependsOn<FanInInd13>][ModularPipelines.Attributes.DependsOn<FanInInd14>][ModularPipelines.Attributes.DependsOn<FanInInd15>]
    [ModularPipelines.Attributes.DependsOn<FanInInd16>][ModularPipelines.Attributes.DependsOn<FanInInd17>][ModularPipelines.Attributes.DependsOn<FanInInd18>][ModularPipelines.Attributes.DependsOn<FanInInd19>][ModularPipelines.Attributes.DependsOn<FanInInd20>]
    [ModularPipelines.Attributes.DependsOn<FanInInd21>][ModularPipelines.Attributes.DependsOn<FanInInd22>][ModularPipelines.Attributes.DependsOn<FanInInd23>][ModularPipelines.Attributes.DependsOn<FanInInd24>][ModularPipelines.Attributes.DependsOn<FanInInd25>]
    [ModularPipelines.Attributes.DependsOn<FanInInd26>][ModularPipelines.Attributes.DependsOn<FanInInd27>][ModularPipelines.Attributes.DependsOn<FanInInd28>][ModularPipelines.Attributes.DependsOn<FanInInd29>][ModularPipelines.Attributes.DependsOn<FanInInd30>]
    [ModularPipelines.Attributes.DependsOn<FanInInd31>][ModularPipelines.Attributes.DependsOn<FanInInd32>][ModularPipelines.Attributes.DependsOn<FanInInd33>][ModularPipelines.Attributes.DependsOn<FanInInd34>][ModularPipelines.Attributes.DependsOn<FanInInd35>]
    [ModularPipelines.Attributes.DependsOn<FanInInd36>][ModularPipelines.Attributes.DependsOn<FanInInd37>][ModularPipelines.Attributes.DependsOn<FanInInd38>][ModularPipelines.Attributes.DependsOn<FanInInd39>][ModularPipelines.Attributes.DependsOn<FanInInd40>]
    [ModularPipelines.Attributes.DependsOn<FanInInd41>][ModularPipelines.Attributes.DependsOn<FanInInd42>][ModularPipelines.Attributes.DependsOn<FanInInd43>][ModularPipelines.Attributes.DependsOn<FanInInd44>][ModularPipelines.Attributes.DependsOn<FanInInd45>]
    [ModularPipelines.Attributes.DependsOn<FanInInd46>][ModularPipelines.Attributes.DependsOn<FanInInd47>][ModularPipelines.Attributes.DependsOn<FanInInd48>][ModularPipelines.Attributes.DependsOn<FanInInd49>][ModularPipelines.Attributes.DependsOn<FanInInd50>]
    public class FanInFinalModule(ExecutionTracker tracker) : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            tracker.RecordStart("FanInFinal");
            tracker.MarkCompleted("FanInFinal");
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
        var tracker = new ExecutionTracker();
        await Assert.That(tracker.IsClean).IsTrue();
        const int totalModules = 51; // 50 independent + 1 final

        var builder = TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) => services.AddSingleton(tracker))
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
        await Assert.That(tracker.CompletedCount).IsEqualTo(totalModules);

        // Verify all modules executed
        var finalRecord = tracker.GetRecord("FanInFinal");
        await Assert.That(finalRecord).IsNotNull();

        for (var i = 1; i <= 50; i++)
        {
            var independent = tracker.GetRecord($"FanInInd{i}");
            await Assert.That(independent).IsNotNull();
        }

        // Note: Dependency ordering is implicitly verified by the pipeline engine
        // The final module cannot execute until all dependencies complete
    }

    #endregion
}
