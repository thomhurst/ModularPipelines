"""Temporary read-only CI diagnostics. Never merge this investigation branch."""
import datetime
import json
import os
import re
import socket
import ssl
import sys
from urllib.parse import urlsplit

ALLOWED_COMMANDS = {"AUTH", "INFO", "HKEYS", "HGET", "ZCARD", "EXISTS", "PTTL"}


def requested_runs():
    run_ids = tuple(value.strip() for value in os.environ["CI_RUN_IDS"].split(","))
    if not 1 <= len(run_ids) <= 5 or any(
        not re.fullmatch(r"[1-9][0-9]{0,19}-[1-9][0-9]{0,5}", value) for value in run_ids
    ):
        raise ValueError("Expected up to five numeric workflow run-attempt identifiers")
    return run_ids


def read_response(stream):
    line = stream.readline(65536)
    if not line.endswith(b"\r\n"):
        raise ValueError("Invalid response")
    kind, value = line[:1], line[1:-2]
    if kind == b"-":
        raise RuntimeError("Redis command failed")
    if kind == b"+":
        return value.decode()
    if kind == b":":
        return int(value)
    if kind == b"$":
        size = int(value)
        if size == -1:
            return None
        if size < 0 or size > 16 * 1024 * 1024:
            raise ValueError("Unexpected response size")
        data = stream.read(size)
        if len(data) != size or stream.read(2) != b"\r\n":
            raise ValueError("Incomplete response")
        return data.decode()
    if kind == b"*":
        count = int(value)
        if count < 0 or count > 10000:
            raise ValueError("Unexpected response count")
        return [read_response(stream) for _ in range(count)]
    raise ValueError("Unsupported response")


def inspect():
    run_ids = requested_runs()
    endpoint = urlsplit("//" + os.environ["REDIS_ENDPOINT"].split(",", 1)[0])
    context = ssl.create_default_context()
    with socket.create_connection((endpoint.hostname, endpoint.port or 6380), timeout=10) as raw:
        with context.wrap_socket(raw, server_hostname=endpoint.hostname) as connection:
            with connection.makefile("rb") as stream:
                def command(name, *arguments):
                    if name not in ALLOWED_COMMANDS:
                        raise ValueError("Command not allowed")
                    parts = [str(value).encode() for value in (name, *arguments)]
                    request = b"*%d\r\n" % len(parts)
                    request += b"".join(b"$%d\r\n" % len(part) + part + b"\r\n" for part in parts)
                    connection.sendall(request)
                    return read_response(stream)

                command("AUTH", os.environ["REDIS_KEY"])
                print("Snapshot UTC:", datetime.datetime.now(datetime.timezone.utc).isoformat())
                for section in ("memory", "stats"):
                    for line in command("INFO", section).splitlines():
                        field, separator, value = line.partition(":")
                        if separator and field in {"used_memory", "maxmemory", "evicted_keys"} and value.isdigit():
                            print(field, int(value))
                for run_id in run_ids:
                    prefix = "modularpipelines-ci:{" + run_id + "}"
                    results = command("HKEYS", prefix + ":results")
                    if any(not re.fullmatch(r"[A-Za-z0-9_.+`]+", name) for name in results):
                        raise ValueError("Unexpected module identifier")
                    print(json.dumps({
                        "run": run_id,
                        "result_count": len(results),
                        "result_ttl_ms": command("PTTL", prefix + ":results"),
                        "queued_count": command("ZCARD", prefix + ":work:queue"),
                        "completion_exists": command("EXISTS", prefix + ":completion"),
                        "cancellation_exists": command("EXISTS", prefix + ":cancellation"),
                        "result_modules": sorted(results),
                    }))
                    for index in range(4):
                        heartbeat = command("HGET", prefix + ":workers", "heartbeat:" + str(index))
                        status = command("HGET", prefix + ":workers:status", index)
                        status = json.loads(status) if status else {}
                        print(json.dumps({
                            "run": run_id,
                            "worker": index,
                            "heartbeat_ms": int(heartbeat) if heartbeat else None,
                            "final_metrics": status.get("UnattributedCommandCount") is not None,
                        }))


if __name__ == "__main__":
    try:
        inspect()
    except Exception as error:
        # Do not print connection settings, Redis payloads, or exception messages.
        print("Diagnostics unavailable:", type(error).__name__)
        sys.exit(1)
