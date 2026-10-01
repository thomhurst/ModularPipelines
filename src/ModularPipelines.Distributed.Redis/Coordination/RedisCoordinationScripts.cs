namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Lua scripts that make each coordination step atomic. Lease entries are JSON objects with
/// <c>LeaseId</c>, <c>WorkerId</c>, the queue <c>Member</c> and <c>Score</c> to requeue, and
/// <c>ExpiresAt</c> in Redis server milliseconds.
/// </summary>
internal static class RedisCoordinationScripts
{
    private const string ServerTimeLua = """
        local server_time = redis.call('TIME')
        local now = (tonumber(server_time[1]) * 1000) + math.floor(tonumber(server_time[2]) / 1000)
        """;

    public const string Claim = ServerTimeLua + """
        if redis.call('EXISTS', KEYS[6]) == 1 then
            return '__closed__'
        end
        local reason = redis.call('GET', KEYS[5])
        if reason == 'Stopped' then
            return '__closed__'
        end
        local always_run_only = reason ~= false
        local caps = cjson.decode(ARGV[1])
        local worker_timeout = tonumber(ARGV[2])
        local priority_band = 1000000000000

        local live_workers = {}
        local worker_entries = redis.call('HGETALL', KEYS[2])
        for i = 1, #worker_entries, 2 do
            local name = worker_entries[i]
            if string.sub(name, 1, 10) ~= 'heartbeat:' then
                local heartbeat = tonumber(redis.call('HGET', KEYS[2], 'heartbeat:' .. name) or '0')
                if heartbeat >= now - worker_timeout then
                    table.insert(live_workers, cjson.decode(worker_entries[i + 1]))
                end
            end
        end

        local function supports(required, available)
            for _, clause in ipairs(required) do
                local found = false
                for _, req in ipairs(clause) do
                    for _, cap in ipairs(available) do
                        if string.lower(req) == string.lower(cap) then
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

        local best_item = nil
        local best_module = nil
        local best_score = nil
        local best_priority = -1
        local best_eligible_workers = nil
        local best_required_count = -1
        local best_weight = -1
        local best_enqueued_at = nil

        local items = redis.call('ZREVRANGE', KEYS[1], 0, -1, 'WITHSCORES')
        for i = 1, #items, 2 do
            local item = items[i]
            local score = tonumber(items[i + 1])
            local separator = string.find(item, '|', 1, true)
            local prefix = separator == 33 and string.sub(item, 1, 32) or nil
            local has_unique_prefix = prefix ~= nil and string.match(prefix, '^[0-9a-fA-F]+$') ~= nil
            local assignment = cjson.decode(has_unique_prefix and string.sub(item, separator + 1) or item)
            local module_id = assignment['ModuleId']
            if redis.call('HEXISTS', KEYS[4], module_id) == 1 or redis.call('HEXISTS', KEYS[3], module_id) == 1 then
                -- A final result or a live lease already exists; drop the stale copy.
                redis.call('ZREM', KEYS[1], item)
            else
                local required = assignment['RequiredCapabilities'] or {}
                if (not always_run_only or assignment['AlwaysRun'] == true) and supports(required, caps) then
                    local eligible_workers = 0
                    for _, worker in ipairs(live_workers) do
                        if supports(required, worker['Capabilities'] or {}) then
                            eligible_workers = eligible_workers + 1
                        end
                    end

                    local priority = math.floor(score / priority_band)
                    local weight = score - (priority * priority_band)
                    local required_count = #required
                    local enqueued_at = assignment['EnqueuedAt'] or ''
                    local is_better = priority > best_priority
                        or (priority == best_priority and (best_eligible_workers == nil or eligible_workers < best_eligible_workers))
                        or (priority == best_priority and eligible_workers == best_eligible_workers and required_count > best_required_count)
                        or (priority == best_priority and eligible_workers == best_eligible_workers and required_count == best_required_count and weight > best_weight)
                        or (priority == best_priority and eligible_workers == best_eligible_workers and required_count == best_required_count and weight == best_weight and (best_enqueued_at == nil or enqueued_at < best_enqueued_at))

                    if is_better then
                        best_item = item
                        best_module = module_id
                        best_score = items[i + 1]
                        best_priority = priority
                        best_eligible_workers = eligible_workers
                        best_required_count = required_count
                        best_weight = weight
                        best_enqueued_at = enqueued_at
                    end
                end
            end
        end

        if best_item == nil then
            return false
        end

        redis.call('ZREM', KEYS[1], best_item)
        redis.call('HSET', KEYS[3], best_module, cjson.encode({
            LeaseId = ARGV[4],
            WorkerId = ARGV[3],
            Member = best_item,
            Score = best_score,
            ExpiresAt = string.format('%d', now + worker_timeout),
        }))
        return best_item
        """;

    public const string Heartbeat = ServerTimeLua + """
        redis.call('HSET', KEYS[1], 'heartbeat:' .. ARGV[1], string.format('%d', now))
        local expires_at = string.format('%d', now + tonumber(ARGV[2]))
        for i = 3, #ARGV do
            local raw = redis.call('HGET', KEYS[2], ARGV[i])
            if raw then
                local lease = cjson.decode(raw)
                if lease['WorkerId'] == ARGV[1] then
                    lease['ExpiresAt'] = expires_at
                    redis.call('HSET', KEYS[2], ARGV[i], cjson.encode(lease))
                end
            end
        end
        return 1
        """;

    public const string MasterHeartbeat = ServerTimeLua + """
        redis.call('SET', KEYS[1], string.format('%d', now), 'PX', ARGV[1])
        return 1
        """;

    // A master that never sent a heartbeat has not started yet, and one that signalled completion
    // finished normally; neither counts as lost.
    public const string IsMasterLost = ServerTimeLua + """
        if redis.call('EXISTS', KEYS[2]) == 1 then
            return 0
        end
        local heartbeat = redis.call('GET', KEYS[1])
        if not heartbeat then
            return 0
        end
        if now - tonumber(heartbeat) > tonumber(ARGV[1]) then
            return 1
        end
        return 0
        """;

    public const string RegisterWorker = ServerTimeLua + """
        local existing = redis.call('HGET', KEYS[1], ARGV[1])
        local same_session = false
        if existing then
            local registration = cjson.decode(existing)
            same_session = registration['RegisteredAt'] == ARGV[3]
            if not same_session then
                local heartbeat = tonumber(redis.call('HGET', KEYS[1], 'heartbeat:' .. ARGV[1]) or '0')
                if heartbeat >= now - tonumber(ARGV[4]) then
                    return 'duplicate'
                end
            end
        end

        redis.call('HSET', KEYS[1], ARGV[1], ARGV[2])
        local status = redis.call('HGET', KEYS[2], ARGV[1])
        local keep_status = false
        if same_session and status then
            local current = cjson.decode(status)
            keep_status = (current['RunId'] or '') == ARGV[6]
        end
        if not keep_status then
            redis.call('HSET', KEYS[2], ARGV[1], ARGV[5])
        end
        redis.call('HSET', KEYS[1], 'heartbeat:' .. ARGV[1], string.format('%d', now))
        return 'ok'
        """;

    public const string PublishResult = """
        local stored = redis.call('HSETNX', KEYS[1], ARGV[1], ARGV[2])
        redis.call('HDEL', KEYS[2], ARGV[1])
        return stored
        """;

    public const string RequeueExpiredLeases = ServerTimeLua + """
        local requeued = {}
        local entries = redis.call('HGETALL', KEYS[1])
        for i = 1, #entries, 2 do
            local lease = cjson.decode(entries[i + 1])
            if tonumber(lease['ExpiresAt']) < now then
                redis.call('HDEL', KEYS[1], entries[i])
                if redis.call('HEXISTS', KEYS[2], entries[i]) == 0 then
                    redis.call('ZADD', KEYS[3], lease['Score'], lease['Member'])
                    table.insert(requeued, entries[i])
                end
            end
        end
        return requeued
        """;

    public const string Withdraw = """
        local removed = 0
        local items = redis.call('ZRANGE', KEYS[1], 0, -1)
        for _, item in ipairs(items) do
            local separator = string.find(item, '|', 1, true)
            local prefix = separator == 33 and string.sub(item, 1, 32) or nil
            local has_unique_prefix = prefix ~= nil and string.match(prefix, '^[0-9a-fA-F]+$') ~= nil
            local assignment = cjson.decode(has_unique_prefix and string.sub(item, separator + 1) or item)
            if assignment['ModuleId'] == ARGV[1] then
                removed = removed + redis.call('ZREM', KEYS[1], item)
            end
        end
        return removed
        """;
}
