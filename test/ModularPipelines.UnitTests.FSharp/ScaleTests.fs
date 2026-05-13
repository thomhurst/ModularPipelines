namespace ModularPipelines.UnitTests.FSharp

open ModularPipelines.TestHelpers
open System.Collections.Concurrent
open System.Reflection
open System.Threading
open ModularPipelines.Modules
open ModularPipelines.Context
open TUnit.Core
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open ModularPipelines.Extensions
open Microsoft.Extensions.DependencyInjection
open ModularPipelines.Enums

module ScaleTestsModule =
    type ExecutionRecord = {
        ModuleName: string
        CompletionOrder : int
        StartTime: System.DateTimeOffset
        EndTime: System.DateTimeOffset
    }
    /// <summary>
    /// Thread-safe tracker for recording module execution order and timing. Each test creates a fresh instance for test
    /// isolation.
    /// </summary>
    type ExecutionTracker() = 
        let _executionRecords = ConcurrentDictionary<string, ExecutionRecord>()
        let mutable _completionCounter = 0

        /// <summary>
        /// Records the start of module execution.
        /// </summary>
        /// <param name="moduleName">The name/type of the module.</param>
        member _.RecordStart(moduleName: string) =
            let record = { ModuleName = moduleName; CompletionOrder = 0; StartTime = System.DateTimeOffset.UtcNow; EndTime = System.DateTimeOffset.MinValue }
            _executionRecords[moduleName] <- record
        /// <summary>
        /// Marks a module as completed and records completion order.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        member _.MarkCompleted(moduleName: string) =
            let completionOrder = Interlocked.Increment(ref _completionCounter);
            match _executionRecords.TryGetValue(moduleName) with
            | true, record ->
                let updatedRecord = { record with CompletionOrder = completionOrder; EndTime = System.DateTimeOffset.UtcNow }
                _executionRecords[moduleName] <- updatedRecord
            | false, _ -> ()

        /// <summary>
        /// Checks if a module has completed.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>True if the module has completed.</returns>
        member _.IsCompleted(moduleName: string) =
            match _executionRecords.TryGetValue(moduleName) with
            | true, record -> record.EndTime <> System.DateTimeOffset.MinValue
            | false, _ -> false

        /// <summary>
        /// Gets all execution records ordered by completion order.
        /// </summary>
        member _.GetRecords() =
            _executionRecords.Values
            |> Seq.sortBy (fun r -> r.CompletionOrder)
            |> Seq.toList
        
        /// <summary>
        /// Gets a specific execution record by module name.
        /// </summary>
        member _.GetRecord(moduleName: string) =
            match _executionRecords.TryGetValue(moduleName) with
            | true, record -> Some record
            | false, _ -> None

        /// <summary>
        /// Gets the count of completed modules.
        /// </summary>
        member _.CompletedCount() =
            _executionRecords.Values |> Seq.sumBy (fun r -> if r.CompletionOrder > 0 then 1 else 0)

        /// <summary>
        /// Gets the total count of recorded modules (started or completed).
        /// </summary>
        member _.TotalCount() = _executionRecords.Count

         /// <summary>
         /// Gets whether the tracker is in a clean state (no records).
         /// </summary>
        member _.IsClean() =
            _executionRecords.IsEmpty && _completionCounter = 0
    
    [<Struct>] type M1 = struct end
    [<Struct>] type M2 = struct end
    [<Struct>] type M3 = struct end
    [<Struct>] type M4 = struct end
    [<Struct>] type M5 = struct end
    [<Struct>] type M6 = struct end
    [<Struct>] type M7 = struct end
    [<Struct>] type M8 = struct end
    [<Struct>] type M9 = struct end
    [<Struct>] type M10 = struct end
    [<Struct>] type M11 = struct end
    [<Struct>] type M12 = struct end
    [<Struct>] type M13 = struct end
    [<Struct>] type M14 = struct end
    [<Struct>] type M15 = struct end
    [<Struct>] type M16 = struct end
    [<Struct>] type M17 = struct end
    [<Struct>] type M18 = struct end
    [<Struct>] type M19 = struct end
    [<Struct>] type M20 = struct end
    [<Struct>] type M21 = struct end
    [<Struct>] type M22 = struct end
    [<Struct>] type M23 = struct end
    [<Struct>] type M24 = struct end
    [<Struct>] type M25 = struct end
    [<Struct>] type M26 = struct end
    [<Struct>] type M27 = struct end
    [<Struct>] type M28 = struct end
    [<Struct>] type M29 = struct end
    [<Struct>] type M30 = struct end
    [<Struct>] type M31 = struct end
    [<Struct>] type M32 = struct end
    [<Struct>] type M33 = struct end
    [<Struct>] type M34 = struct end
    [<Struct>] type M35 = struct end
    [<Struct>] type M36 = struct end
    [<Struct>] type M37 = struct end
    [<Struct>] type M38 = struct end
    [<Struct>] type M39 = struct end
    [<Struct>] type M40 = struct end
    [<Struct>] type M41 = struct end
    [<Struct>] type M42 = struct end
    [<Struct>] type M43 = struct end
    [<Struct>] type M44 = struct end
    [<Struct>] type M45 = struct end
    [<Struct>] type M46 = struct end
    [<Struct>] type M47 = struct end
    [<Struct>] type M48 = struct end
    [<Struct>] type M49 = struct end
    [<Struct>] type M50 = struct end
    [<Struct>] type M51 = struct end
    [<Struct>] type M52 = struct end
    [<Struct>] type M53 = struct end
    [<Struct>] type M54 = struct end
    [<Struct>] type M55 = struct end
    [<Struct>] type M56 = struct end
    [<Struct>] type M57 = struct end
    [<Struct>] type M58 = struct end
    [<Struct>] type M59 = struct end
    [<Struct>] type M60 = struct end
    [<Struct>] type M61 = struct end
    [<Struct>] type M62 = struct end
    [<Struct>] type M63 = struct end
    [<Struct>] type M64 = struct end
    [<Struct>] type M65 = struct end
    [<Struct>] type M66 = struct end
    [<Struct>] type M67 = struct end
    [<Struct>] type M68 = struct end
    [<Struct>] type M69 = struct end
    [<Struct>] type M70 = struct end
    [<Struct>] type M71 = struct end
    [<Struct>] type M72 = struct end
    [<Struct>] type M73 = struct end
    [<Struct>] type M74 = struct end
    [<Struct>] type M75 = struct end
    [<Struct>] type M76 = struct end
    [<Struct>] type M77 = struct end
    [<Struct>] type M78 = struct end
    [<Struct>] type M79 = struct end
    [<Struct>] type M80 = struct end
    [<Struct>] type M81 = struct end
    [<Struct>] type M82 = struct end
    [<Struct>] type M83 = struct end
    [<Struct>] type M84 = struct end
    [<Struct>] type M85 = struct end
    [<Struct>] type M86 = struct end
    [<Struct>] type M87 = struct end
    [<Struct>] type M88 = struct end
    [<Struct>] type M89 = struct end
    [<Struct>] type M90 = struct end
    [<Struct>] type M91 = struct end
    [<Struct>] type M92 = struct end
    [<Struct>] type M93 = struct end
    [<Struct>] type M94 = struct end
    [<Struct>] type M95 = struct end
    [<Struct>] type M96 = struct end
    [<Struct>] type M97 = struct end
    [<Struct>] type M98 = struct end
    [<Struct>] type M99 = struct end
    [<Struct>] type M100 = struct end

    /// <summary>
    /// A generic module that can be instantiated with different type markers to create unique types.
    /// </summary>
    /// <typeparam name="T">A marker type that makes this module unique.</typeparam>
    type ScaleModule<'T>(tracker: ExecutionTracker) =
        inherit Module<string>()
        override _.ExecuteAsync(context: IModuleContext, cancellationToken: System.Threading.CancellationToken) =
            task {
                let typeName = typeof<'T>.Name
                tracker.RecordStart(typeName);
                tracker.MarkCompleted(typeName);
                return typeName;
            }
    [<Struct>] type C1 = struct end
    [<Struct>] type C2 = struct end
    [<Struct>] type C3 = struct end
    [<Struct>] type C4 = struct end
    [<Struct>] type C5 = struct end
    [<Struct>] type C6 = struct end
    [<Struct>] type C7 = struct end
    [<Struct>] type C8 = struct end
    [<Struct>] type C9 = struct end
    [<Struct>] type C10 = struct end
    [<Struct>] type C11 = struct end
    [<Struct>] type C12 = struct end
    [<Struct>] type C13 = struct end
    [<Struct>] type C14 = struct end
    [<Struct>] type C15 = struct end
    [<Struct>] type C16 = struct end
    [<Struct>] type C17 = struct end
    [<Struct>] type C18 = struct end
    [<Struct>] type C19 = struct end
    [<Struct>] type C20 = struct end
    [<Struct>] type C21 = struct end
    [<Struct>] type C22 = struct end
    [<Struct>] type C23 = struct end
    [<Struct>] type C24 = struct end
    [<Struct>] type C25 = struct end
    [<Struct>] type C26 = struct end
    [<Struct>] type C27 = struct end
    [<Struct>] type C28 = struct end
    [<Struct>] type C29 = struct end
    [<Struct>] type C30 = struct end
    [<Struct>] type C31 = struct end
    [<Struct>] type C32 = struct end
    [<Struct>] type C33 = struct end
    [<Struct>] type C34 = struct end
    [<Struct>] type C35 = struct end
    [<Struct>] type C36 = struct end
    [<Struct>] type C37 = struct end
    [<Struct>] type C38 = struct end
    [<Struct>] type C39 = struct end
    [<Struct>] type C40 = struct end
    [<Struct>] type C41 = struct end
    [<Struct>] type C42 = struct end
    [<Struct>] type C43 = struct end
    [<Struct>] type C44 = struct end
    [<Struct>] type C45 = struct end
    [<Struct>] type C46 = struct end
    [<Struct>] type C47 = struct end
    [<Struct>] type C48 = struct end
    [<Struct>] type C49 = struct end
    [<Struct>] type C50 = struct end

    // Chain modules - each depends on the previous one
    type ChainModule1(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain1")
                tracker.MarkCompleted("Chain1")
                return 1
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule1>)>]
    type ChainModule2(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain2")
                tracker.MarkCompleted("Chain2")
                return 2
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule2>)>]
    type ChainModule3(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain3")
                tracker.MarkCompleted("Chain3")
                return 3
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule3>)>]
    type ChainModule4(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain4")
                tracker.MarkCompleted("Chain4")
                return 4
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule4>)>]
    type ChainModule5(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain5")
                tracker.MarkCompleted("Chain5")
                return 5
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule5>)>]
    type ChainModule6(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain6")
                tracker.MarkCompleted("Chain6")
                return 6
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule6>)>]
    type ChainModule7(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain7")
                tracker.MarkCompleted("Chain7")
                return 7
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule7>)>]
    type ChainModule8(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain8")
                tracker.MarkCompleted("Chain8")
                return 8
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule8>)>]
    type ChainModule9(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain9")
                tracker.MarkCompleted("Chain9")
                return 9
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule9>)>]
    type ChainModule10(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain10")
                tracker.MarkCompleted("Chain10")
                return 10
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule10>)>]
    type ChainModule11(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain11")
                tracker.MarkCompleted("Chain11")
                return 11
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule11>)>]
    type ChainModule12(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain12")
                tracker.MarkCompleted("Chain12")
                return 12
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule12>)>]
    type ChainModule13(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain13")
                tracker.MarkCompleted("Chain13")
                return 13
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule13>)>]
    type ChainModule14(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain14")
                tracker.MarkCompleted("Chain14")
                return 14
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule14>)>]
    type ChainModule15(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain15")
                tracker.MarkCompleted("Chain15")
                return 15
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule15>)>]
    type ChainModule16(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain16")
                tracker.MarkCompleted("Chain16")
                return 16
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule16>)>]
    type ChainModule17(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain17")
                tracker.MarkCompleted("Chain17")
                return 17
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule17>)>]
    type ChainModule18(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain18")
                tracker.MarkCompleted("Chain18")
                return 18
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule18>)>]
    type ChainModule19(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain19")
                tracker.MarkCompleted("Chain19")
                return 19
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule19>)>]
    type ChainModule20(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain20")
                tracker.MarkCompleted("Chain20")
                return 20
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule20>)>]
    type ChainModule21(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain21")
                tracker.MarkCompleted("Chain21")
                return 21
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule21>)>]
    type ChainModule22(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain22")
                tracker.MarkCompleted("Chain22")
                return 22
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule22>)>]
    type ChainModule23(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain23")
                tracker.MarkCompleted("Chain23")
                return 23
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule23>)>]
    type ChainModule24(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain24")
                tracker.MarkCompleted("Chain24")
                return 24
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule24>)>]
    type ChainModule25(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain25")
                tracker.MarkCompleted("Chain25")
                return 25
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule25>)>]
    type ChainModule26(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain26")
                tracker.MarkCompleted("Chain26")
                return 26
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule26>)>]
    type ChainModule27(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain27")
                tracker.MarkCompleted("Chain27")
                return 27
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule27>)>]
    type ChainModule28(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain28")
                tracker.MarkCompleted("Chain28")
                return 28
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule28>)>]
    type ChainModule29(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain29")
                tracker.MarkCompleted("Chain29")
                return 29
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule29>)>]
    type ChainModule30(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain30")
                tracker.MarkCompleted("Chain30")
                return 30
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule30>)>]
    type ChainModule31(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain31")
                tracker.MarkCompleted("Chain31")
                return 31
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule31>)>]
    type ChainModule32(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain32")
                tracker.MarkCompleted("Chain32")
                return 32
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule32>)>]
    type ChainModule33(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain33")
                tracker.MarkCompleted("Chain33")
                return 33
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule33>)>]
    type ChainModule34(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain34")
                tracker.MarkCompleted("Chain34")
                return 34
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule34>)>]
    type ChainModule35(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain35")
                tracker.MarkCompleted("Chain35")
                return 35
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule35>)>]
    type ChainModule36(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain36")
                tracker.MarkCompleted("Chain36")
                return 36
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule36>)>]
    type ChainModule37(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain37")
                tracker.MarkCompleted("Chain37")
                return 37
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule37>)>]
    type ChainModule38(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain38")
                tracker.MarkCompleted("Chain38")
                return 38
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule38>)>]
    type ChainModule39(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain39")
                tracker.MarkCompleted("Chain39")
                return 39
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule39>)>]
    type ChainModule40(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain40")
                tracker.MarkCompleted("Chain40")
                return 40
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule40>)>]
    type ChainModule41(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain41")
                tracker.MarkCompleted("Chain41")
                return 41
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule41>)>]
    type ChainModule42(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain42")
                tracker.MarkCompleted("Chain42")
                return 42
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule42>)>]
    type ChainModule43(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain43")
                tracker.MarkCompleted("Chain43")
                return 43
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule43>)>]
    type ChainModule44(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain44")
                tracker.MarkCompleted("Chain44")
                return 44
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule44>)>]
    type ChainModule45(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain45")
                tracker.MarkCompleted("Chain45")
                return 45
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule45>)>]
    type ChainModule46(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain46")
                tracker.MarkCompleted("Chain46")
                return 46
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule46>)>]
    type ChainModule47(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain47")
                tracker.MarkCompleted("Chain47")
                return 47
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule47>)>]
    type ChainModule48(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain48")
                tracker.MarkCompleted("Chain48")
                return 48
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule48>)>]
    type ChainModule49(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain49")
                tracker.MarkCompleted("Chain49")
                return 49
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule49>)>]
    type ChainModule50(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain50")
                tracker.MarkCompleted("Chain50")
                return 50
            }

    /// <summary>
    /// Tests for large-scale pipeline scenarios to validate scalability, performance, and correct behavior with many
    /// modules.
    /// </summary>
    /// <remarks>
    /// These tests verify that the pipeline engine handles large numbers of modules correctly, including proper
    /// parallelization, dependency ordering, and resource management. Note: ModularPipelines requires each module to be
    /// a unique type (by design, dependencies are resolved by type). These tests use generic types with different type
    /// parameters to create many unique module types at compile time.
    /// </remarks>
    [<NotInParallel>]
    type ScaleTests() =
        inherit TestBase()

        /// <summary>
        /// Verifies that a pipeline with 100 independent modules completes successfully and all modules execute.
        /// </summary>
        /// <remarks>
        /// This test validates: - The pipeline can handle 100 modules without issues - All modules complete
        /// successfully - The pipeline status is successful
        /// </remarks>
        [<Test>]
        member _.Pipeline_With100IndependentModules_CompletesSuccessfully() =
            async {
                let tracker = new ExecutionTracker();
                do! check(Assert.That<bool>(tracker.IsClean).IsTrue())
                let expectedModuleCount = 100
                let builder =
                    TestPipelineHostBuilder.Create()
                        .ConfigureServices(fun _ services -> services.AddSingleton(tracker) |> ignore)
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
                        .AddModule<ScaleModule<M100>>()

                let! pipelineSummary = builder.ExecutePipelineAsync() |> Async.AwaitTask
                do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
                do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(tracker.CompletedCount()), expectedModuleCount))
            }
       
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
        [<Test>]
        member _.Pipeline_With50ModuleDeepChain_CompletesInOrder() =
         async{
            let tracker = new ExecutionTracker();
            do! check(Assert.That<bool>(tracker.IsClean).IsTrue())
            let chainDepth = 50;
            let builder =
                TestPipelineHostBuilder.Create()
                    .ConfigureServices(fun _ services -> services.AddSingleton(tracker) |> ignore)
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
            let! pipelineSummary = builder.ExecutePipelineAsync() |> Async.AwaitTask
            do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
            do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(tracker.CompletedCount()), chainDepth))

            // Verify all chain modules executed
            for i = 1 to chainDepth do
                let record = tracker.GetRecord($"Chain{i}")
                do! check(Assert.That(record.IsSome).IsTrue())

            // Verify dependency ordering: each module must complete before its dependent
            // Chain1 -> Chain2 -> Chain3 -> ... -> Chain50
            // Get all records sorted by completion order for diagnostics
            let orderedRecords =
                [ 1 .. chainDepth ]
                |> List.choose (fun i -> tracker.GetRecord($"Chain{i}"))
                |> List.sortBy (fun r -> r.CompletionOrder)

            // Verify the completion order matches the dependency order
            for i, actualRecord in orderedRecords |> List.indexed do
                let expectedModuleName = $"Chain{i + 1}"

                do! check(
                    StringEqualsAssertionExtensions.IsEqualTo(
                        Assert.That(actualRecord.ModuleName),
                        expectedModuleName
                    ).Because($"Module at completion order {i + 1} should be {expectedModuleName} but was {actualRecord.ModuleName}")
                )
         }