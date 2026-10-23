namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Lua scripts that make each coordination step atomic. Every script receives
/// <see cref="RedisKeyBuilder.ScriptKeys"/> as <c>KEYS</c>:
/// <list type="number">
/// <item>work queue: a sorted set whose members all have score zero and sort as
/// <c>{class}|{order}|{assignment}</c> (see <see cref="RedisQueueMember"/>)</item>
/// <item>queue classes: class identifier to <c>{RequiredCapabilities, AlwaysRun}</c></item>
/// <item>queue index: module identifier to its queued members, separated by newlines</item>
/// <item>leases: module identifier to <c>{LeaseId, WorkerId, Member, ExpiresAt, Renewed}</c>, with
/// <c>ExpiresAt</c> in Redis server milliseconds</item>
/// <item>results</item>
/// <item>worker registrations</item>
/// <item>worker heartbeats, whose fields expire after the worker timeout (Redis 7.4+)</item>
/// <item>worker statuses</item>
/// <item>completion flag</item>
/// <item>cancellation flag</item>
/// <item>master started marker</item>
/// <item>lease expiration workers, keyed by module identifier</item>
/// <item>master heartbeat</item>
/// <item>claims: claim token to module identifier</item>
/// <item>enqueue receipts: the unique suffix of each committed enqueue request</item>
/// </list>
/// Scripts that change run state renew the expiry of keys 1 to 12, 14 and 15.
/// </summary>
internal static class RedisCoordinationScripts
{
    private const string ServerTimeLua = """
        local server_time = redis.call('TIME')
        local now = (tonumber(server_time[1]) * 1000) + math.floor(tonumber(server_time[2]) / 1000)

        """;

    private const string RefreshLua = """
        local function refresh(ttl)
            for i = 1, 12 do
                redis.call('PEXPIRE', KEYS[i], ttl)
            end
            redis.call('PEXPIRE', KEYS[14], ttl)
            redis.call('PEXPIRE', KEYS[15], ttl)
        end

        """;

    private const string QueueIndexLua = """
        local function index_add(module_id, member)
            local current = redis.call('HGET', KEYS[3], module_id)
            if not current then
                redis.call('HSET', KEYS[3], module_id, member)
                return
            end
            for existing in string.gmatch(current, '[^\n]+') do
                if existing == member then
                    return
                end
            end
            redis.call('HSET', KEYS[3], module_id, current .. '\n' .. member)
        end

        local function index_remove(module_id, member)
            local current = redis.call('HGET', KEYS[3], module_id)
            if not current then
                return
            end
            local kept = {}
            for existing in string.gmatch(current, '[^\n]+') do
                if existing ~= member then
                    table.insert(kept, existing)
                end
            end
            if #kept == 0 then
                redis.call('HDEL', KEYS[3], module_id)
            else
                redis.call('HSET', KEYS[3], module_id, table.concat(kept, '\n'))
            end
        end

        """;

    // ARGV: time to live, class identifier, class JSON, queue member, module identifier.
    public const string Enqueue = RefreshLua + QueueIndexLua + """
        -- Bytes 53-84 are the queue member's unique GUID suffix (see RedisQueueMember).
        -- Keep a receipt even after withdrawal or claim removes the member from the queue.
        local request_id = string.sub(ARGV[4], 53, 84)
        if redis.call('SISMEMBER', KEYS[15], request_id) == 1 then
            refresh(ARGV[1])
            return 0
        end
        redis.call('HSETNX', KEYS[2], ARGV[2], ARGV[3])
        redis.call('ZADD', KEYS[1], 0, ARGV[4])
        index_add(ARGV[5], ARGV[4])
        redis.call('SADD', KEYS[15], request_id)
        refresh(ARGV[1])
        return 1
        """;

    // ARGV: worker capabilities JSON, worker identifier, lease/claim identifier, worker timeout, time to live.
    // Each class is a contiguous range of the queue whose first member is that class's best item, so a
    // claim reads one member per class instead of scanning the queue. Returns the claimed assignment JSON,
    // '__closed__' once the run stops handing out work, '__normalize__' followed by legacy metadata
    // that .NET must normalize before retrying, or nil when nothing can be claimed.
    public const string Claim = ServerTimeLua + RefreshLua + QueueIndexLua + """
        if redis.call('EXISTS', KEYS[9]) == 1 then
            return '__closed__'
        end
        local reason = redis.call('GET', KEYS[10])
        if reason == 'Stopped' then
            return '__closed__'
        end
        local always_run_only = reason ~= false
        local claimed_module = redis.call('HGET', KEYS[14], ARGV[3])
        if claimed_module then
            local raw = redis.call('HGET', KEYS[4], claimed_module)
            local lease = raw and cjson.decode(raw)
            if lease and lease['LeaseId'] == ARGV[3] and lease['WorkerId'] == ARGV[2]
                and tonumber(lease['ExpiresAt']) >= now then
                local assignment = string.sub(lease['Member'], 86)
                -- A lost response may be retried after the pipeline fails.
                if always_run_only and cjson.decode(assignment)['AlwaysRun'] ~= true then
                    return false
                end
                refresh(ARGV[5])
                return assignment
            end
            -- Never let a late retry claim different work or resurrect an expired lease.
            return false
        end
        local caps = cjson.decode(ARGV[1])
        local worker_timeout = tonumber(ARGV[4])

        -- Return legacy metadata before changing queue state. The caller normalizes in .NET and
        -- conditionally rewrites these exact values, then retries this atomic snapshot.
        local legacy = {}
        local classes = redis.call('HGETALL', KEYS[2])
        for i = 1, #classes, 2 do
            local class = cjson.decode(classes[i + 1])
            if class['CapabilitiesNormalized'] ~= true then
                table.insert(legacy, { KeyIndex = 2, Field = classes[i], Value = classes[i + 1] })
            end
            classes[i + 1] = class
        end
        local live_workers = {}
        -- Expired heartbeat fields are gone, so this hash lists exactly the live workers.
        for _, id in ipairs(redis.call('HKEYS', KEYS[7])) do
            local raw = redis.call('HGET', KEYS[6], id)
            if raw then
                local registration = cjson.decode(raw)
                if registration['NormalizedCapabilities'] == nil then
                    table.insert(legacy, { KeyIndex = 6, Field = id, Value = raw })
                else
                    table.insert(live_workers, registration['NormalizedCapabilities'])
                end
            end
        end
        if #legacy > 0 then
            return '__normalize__' .. cjson.encode(legacy)
        end

        -- Both sides carry .NET OrdinalIgnoreCase comparison keys; Lua cannot case-fold Unicode.
        local function supports(required, available)
            for _, clause in ipairs(required) do
                local found = false
                for _, req in ipairs(clause) do
                    for _, cap in ipairs(available) do
                        if req == cap then
                            found = true
                            break
                        end
                    end
                    if found then
                        break
                    end
                end
                if not found then
                    return false
                end
            end
            return true
        end

        local best = nil
        for i = 1, #classes, 2 do
            local class_id = classes[i]
            local class = classes[i + 1]
            local required = class['RequiredCapabilities'] or {}
            if (not always_run_only or class['AlwaysRun'] == true) and supports(required, caps) then
                local head = nil
                while true do
                    local items = redis.call('ZRANGE', KEYS[1], '[' .. class_id .. '|', '[' .. class_id .. '|\255', 'BYLEX', 'LIMIT', 0, 1)
                    if #items == 0 then
                        break
                    end
                    local member = items[1]
                    local module_id = cjson.decode(string.sub(member, 86))['ModuleId']
                    if redis.call('HEXISTS', KEYS[5], module_id) == 1 or redis.call('HEXISTS', KEYS[4], module_id) == 1 then
                        -- A final result or a live lease already exists; drop the stale copy.
                        redis.call('ZREM', KEYS[1], member)
                        index_remove(module_id, member)
                    else
                        head = { member = member, module_id = module_id }
                        break
                    end
                end

                if head ~= nil then
                    local eligible_workers = 0
                    for _, worker_caps in ipairs(live_workers) do
                        if supports(required, worker_caps) then
                            eligible_workers = eligible_workers + 1
                        end
                    end

                    -- The order starts with the inverted priority, then the inverted critical path weight and
                    -- the enqueue time. Scarce classes win ties on priority, then classes with more requirements.
                    local order = string.sub(head.member, 18, 84)
                    local priority = string.sub(order, 1, 1)
                    local required_count = #required
                    local is_better = best == nil
                        or priority < best.priority
                        or (priority == best.priority and eligible_workers < best.eligible_workers)
                        or (priority == best.priority and eligible_workers == best.eligible_workers and required_count > best.required_count)
                        or (priority == best.priority and eligible_workers == best.eligible_workers and required_count == best.required_count and order < best.order)
                    if is_better then
                        best = {
                            member = head.member,
                            module_id = head.module_id,
                            priority = priority,
                            eligible_workers = eligible_workers,
                            required_count = required_count,
                            order = order,
                        }
                    end
                end
            end
        end

        if best == nil then
            return false
        end

        redis.call('ZREM', KEYS[1], best.member)
        index_remove(best.module_id, best.member)
        redis.call('HSET', KEYS[4], best.module_id, cjson.encode({
            LeaseId = ARGV[3],
            WorkerId = ARGV[2],
            Member = best.member,
            ExpiresAt = string.format('%d', now + worker_timeout),
            Renewed = false,
        }))
        redis.call('HSET', KEYS[14], ARGV[3], best.module_id)
        refresh(ARGV[5])
        return string.sub(best.member, 86)
        """;

    // ARGV: groups of key index, field, original JSON, normalized JSON. Never recreate deleted
    // state or overwrite a registration/class that changed since the claim returned it.
    public const string NormalizeCapabilities = """
        for i = 1, #ARGV, 4 do
            local key = KEYS[tonumber(ARGV[i])]
            if redis.call('HGET', key, ARGV[i + 1]) == ARGV[i + 2] then
                redis.call('HSET', key, ARGV[i + 1], ARGV[i + 3])
            end
        end
        return 1
        """;

    // Same ARGV as Claim. Acknowledgement proves that a claim response reached its caller, so
    // a crash before the first periodic heartbeat still counts toward the expiration budget.
    public const string AcknowledgeClaim = ServerTimeLua + RefreshLua + """
        local module_id = redis.call('HGET', KEYS[14], ARGV[3])
        local raw = module_id and redis.call('HGET', KEYS[4], module_id)
        local lease = raw and cjson.decode(raw)
        if not lease or lease['LeaseId'] ~= ARGV[3] or lease['WorkerId'] ~= ARGV[2]
            or tonumber(lease['ExpiresAt']) < now then
            return false
        end
        -- A lost acknowledgement response must not keep unstarted work alive indefinitely.
        -- Only delivery's first acknowledgement extends the deadline; heartbeats renew execution.
        if lease['Renewed'] ~= true then
            lease['Renewed'] = true
            lease['ExpiresAt'] = string.format('%d', now + tonumber(ARGV[4]))
            redis.call('HSET', KEYS[4], module_id, cjson.encode(lease))
        end
        refresh(ARGV[5])
        return string.sub(lease['Member'], 86)
        """;

    // ARGV: time to live, worker identifier, worker timeout, status JSON, then in-flight module identifiers.
    public const string Heartbeat = ServerTimeLua + RefreshLua + """
        redis.call('HSET', KEYS[8], ARGV[2], ARGV[4])
        redis.call('HSET', KEYS[7], ARGV[2], string.format('%d', now))
        redis.call('HPEXPIRE', KEYS[7], ARGV[3], 'FIELDS', 1, ARGV[2])
        local expires_at = string.format('%d', now + tonumber(ARGV[3]))
        for i = 5, #ARGV do
            local raw = redis.call('HGET', KEYS[4], ARGV[i])
            if raw then
                local lease = cjson.decode(raw)
                if lease['WorkerId'] == ARGV[2] then
                    lease['ExpiresAt'] = expires_at
                    lease['Renewed'] = true
                    redis.call('HSET', KEYS[4], ARGV[i], cjson.encode(lease))
                end
            end
        end
        refresh(ARGV[1])
        return 1
        """;

    // ARGV: time to live. Reserve before heartbeats or dispatch, without changing a rejected run.
    // Workers may already have registered, so their state does not make a run used.
    public const string ReserveMasterRun = ServerTimeLua + """
        if redis.call('EXISTS', KEYS[11], KEYS[9], KEYS[5]) > 0 then
            return 0
        end
        redis.call('SET', KEYS[11], '1', 'PX', ARGV[1])
        redis.call('SET', KEYS[13], string.format('%d', now), 'PX', ARGV[1])
        return 1
        """;

    // ARGV: time to live, worker identifier, worker timeout, status JSON, lease tokens JSON.
    public const string ReconcileHeartbeat = ServerTimeLua + RefreshLua + """
        redis.call('HSET', KEYS[8], ARGV[2], ARGV[4])
        redis.call('HSET', KEYS[7], ARGV[2], string.format('%d', now))
        redis.call('HPEXPIRE', KEYS[7], ARGV[3], 'FIELDS', 1, ARGV[2])
        local lost = {}
        for _, token in ipairs(cjson.decode(ARGV[5])) do
            local raw = redis.call('HGET', KEYS[4], token['ModuleId'])
            local lease = raw and cjson.decode(raw)
            if lease and lease['WorkerId'] == ARGV[2]
                and token['WorkerId'] == ARGV[2] and lease['LeaseId'] == token['LeaseId']
                and tonumber(lease['ExpiresAt']) >= now then
                lease['ExpiresAt'] = string.format('%d', now + tonumber(ARGV[3]))
                lease['Renewed'] = true
                redis.call('HSET', KEYS[4], token['ModuleId'], cjson.encode(lease))
            else
                table.insert(lost, token['LeaseId'])
            end
        end
        refresh(ARGV[1])
        return lost
        """;

    // ARGV: heartbeat time to live. The heartbeat is KEYS[13] and the durable started marker KEYS[11].
    public const string MasterHeartbeat = ServerTimeLua + """
        redis.call('SET', KEYS[13], string.format('%d', now), 'PX', ARGV[1])
        redis.call('SET', KEYS[11], '1', 'PX', ARGV[1])
        return 1
        """;

    // ARGV: master timeout. Returns 0 while the master runs, 1 once it signalled completion and 2 once it
    // is lost. A master that started is lost once its heartbeat is stale or has expired; a master that
    // never sent a heartbeat has not started yet.
    public const string GetMasterState = ServerTimeLua + """
        if redis.call('EXISTS', KEYS[9]) == 1 then
            return 1
        end
        local heartbeat = redis.call('GET', KEYS[13])
        if not heartbeat then
            if redis.call('EXISTS', KEYS[11]) == 1 then
                return 2
            end
            return 0
        end
        if now - tonumber(heartbeat) > tonumber(ARGV[1]) then
            return 2
        end
        return 0
        """;

    // ARGV: time to live, worker identifier, registration JSON, registration time, worker timeout,
    // initial status JSON, run identifier.
    public const string RegisterWorker = ServerTimeLua + RefreshLua + """
        local existing = redis.call('HGET', KEYS[6], ARGV[2])
        local same_session = false
        if existing then
            local registration = cjson.decode(existing)
            same_session = registration['RegisteredAt'] == ARGV[4]
        end
        if not same_session then
            if redis.call('EXISTS', KEYS[9]) == 1 then
                return 'completed'
            end
            if existing and redis.call('HEXISTS', KEYS[7], ARGV[2]) == 1 then
                return 'duplicate'
            end
        end

        redis.call('HSET', KEYS[6], ARGV[2], ARGV[3])
        local status = redis.call('HGET', KEYS[8], ARGV[2])
        local keep_status = false
        if same_session and status then
            local current = cjson.decode(status)
            keep_status = (current['RunId'] or '') == ARGV[7]
        end
        if not keep_status then
            redis.call('HSET', KEYS[8], ARGV[2], ARGV[6])
        end
        redis.call('HSET', KEYS[7], ARGV[2], string.format('%d', now))
        redis.call('HPEXPIRE', KEYS[7], ARGV[5], 'FIELDS', 1, ARGV[2])
        refresh(ARGV[1])
        return 'ok'
        """;

    // ARGV: time to live, module identifier, result JSON, lease identifier (empty for master), worker identifier.
    public const string PublishResult = ServerTimeLua + RefreshLua + """
        if ARGV[4] ~= '' then
            local raw = redis.call('HGET', KEYS[4], ARGV[2])
            local lease = raw and cjson.decode(raw)
            if not lease or lease['LeaseId'] ~= ARGV[4] or lease['WorkerId'] ~= ARGV[5]
                or tonumber(lease['ExpiresAt']) < now then
                return 0
            end
        end
        local stored = redis.call('HSETNX', KEYS[5], ARGV[2], ARGV[3])
        redis.call('HDEL', KEYS[4], ARGV[2])
        redis.call('HDEL', KEYS[12], ARGV[2])
        refresh(ARGV[1])
        return stored
        """;

    // ARGV: time to live, expiration budget, completion timestamp.
    // Returns requeued identifiers and failed identifiers separately, for waiter notifications.
    public const string RequeueExpiredLeases = ServerTimeLua + RefreshLua + QueueIndexLua + """
        local requeued = {}
        local failed = {}
        local entries = redis.call('HGETALL', KEYS[4])
        for i = 1, #entries, 2 do
            local lease = cjson.decode(entries[i + 1])
            if tonumber(lease['ExpiresAt']) < now then
                redis.call('HDEL', KEYS[4], entries[i])
                if redis.call('HEXISTS', KEYS[5], entries[i]) == 0 then
                    local history = redis.call('HGET', KEYS[12], entries[i])
                    local workers = history and cjson.decode(history) or {}
                    -- An unrenewed lease may be a claim whose response was lost or cancelled.
                    if lease['Renewed'] == true then
                        table.insert(workers, lease['WorkerId'])
                    end
                    if #workers >= tonumber(ARGV[2]) then
                        redis.call('HSET', KEYS[5], entries[i], cjson.encode({
                            ModuleId = entries[i],
                            LeaseExpirationWorkers = workers,
                            CompletedAt = ARGV[3],
                        }))
                        redis.call('HDEL', KEYS[12], entries[i])
                        local queued = redis.call('HGET', KEYS[3], entries[i])
                        if queued then
                            for member in string.gmatch(queued, '[^\n]+') do
                                redis.call('ZREM', KEYS[1], member)
                            end
                            redis.call('HDEL', KEYS[3], entries[i])
                        end
                        table.insert(failed, entries[i])
                    else
                        redis.call('HSET', KEYS[12], entries[i], cjson.encode(workers))
                        redis.call('ZADD', KEYS[1], 0, lease['Member'])
                        index_add(entries[i], lease['Member'])
                        table.insert(requeued, entries[i])
                    end
                end
            end
        end
        refresh(ARGV[1])
        return { requeued, failed }
        """;

    // ARGV: module identifier. Returns how many queued copies were removed.
    public const string Withdraw = """
        local members = redis.call('HGET', KEYS[3], ARGV[1])
        if not members then
            return 0
        end
        local removed = 0
        for member in string.gmatch(members, '[^\n]+') do
            removed = removed + redis.call('ZREM', KEYS[1], member)
        end
        redis.call('HDEL', KEYS[3], ARGV[1])
        return removed
        """;
}
