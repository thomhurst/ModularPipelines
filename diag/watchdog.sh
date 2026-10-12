#!/bin/bash
# Runs as root, reparented to launchd. Captures diagnostics when the repro heartbeat stalls,
# then uploads them itself, because a frozen runner cannot upload artifacts.
set -u
hb="$1"; out="$2"; tag="$3"; label="$4"; dotnet_root="$5"
export PATH="/opt/homebrew/bin:/usr/local/bin:/usr/bin:/bin:/usr/sbin:/sbin"
to() { perl -e 'alarm shift; exec @ARGV' "$@"; }
mkdir -p "$out"
log="$out/watchdog.log"
echo "watchdog pid $$ started $(date -u +%T)" > "$log"
while true; do
  sleep 5
  [ -f "$hb" ] || continue
  if grep -q "completed 200 iterations" "$hb"; then
    echo "repro completed $(date -u +%T); Runner.Listener: $(pgrep -f Runner.Listener || echo GONE)" >> "$log"
    ps -axo pid,ppid,pgid,user,stat,wchan,etime,command > "$out/ps-after.txt"
    cp "$hb" "$out/heartbeat.txt"
    ( cd "$(dirname "$out")" && tar czf "/tmp/$label.tar.gz" "$(basename "$out")" )
    gh release upload "$tag" "/tmp/$label.tar.gz" --clobber -R "$GITHUB_REPOSITORY" >> "$log" 2>&1
    gh api "repos/$GITHUB_REPOSITORY/issues/5926/comments" -f body="$(printf 'dump capture %s (run %s)
```
%s
--- heartbeat signals ---
%s
```' "$label" "$GITHUB_RUN_ID" "$(cat "$log")" "$(grep -E 'SIG|iteration 1:|completed' "$hb" | head -40)")" > /dev/null 2>&1
    exit 0
  fi
  age=$(( $(date +%s) - $(stat -f %m "$hb") ))
  (( age < 20 )) && continue
  pid=$(head -1 "$hb" | awk '{print $2}')
  echo "STALL detected $(date -u +%T): heartbeat ${age}s old, repro pid $pid" >> "$log"
  tail -3 "$hb" >> "$log"
  ps -axo pid,ppid,pgid,user,stat,wchan,etime,command > "$out/ps-before.txt"
  to 60 sample "$pid" 5 -file "$out/sample-repro.txt" >> "$log" 2>&1
  to 120 spindump "$pid" 5 10 -file "$out/spindump-repro.txt" >> "$log" 2>&1
  to 120 spindump -notarget 5 10 -file "$out/spindump-system.txt" >> "$log" 2>&1
  createdump=$(ls -d "$dotnet_root"/shared/Microsoft.NETCore.App/10.*/createdump 2>/dev/null | sort -V | tail -1)
  echo "createdump: $createdump" >> "$log"
  [ -n "$createdump" ] && to 300 "$createdump" --withheap -f "$out/repro.%p.dmp" "$pid" >> "$log" 2>&1
  stopped=$(ps -axo pid=,stat=,user= | awk '$3=="runner" && $2 ~ /T/ {print $1}' | tr '\n' ' ')
  echo "stopped runner-user processes: ${stopped:-none}" >> "$log"
  if [ -n "$stopped" ]; then kill -CONT $stopped; echo "sent SIGCONT $(date -u +%T)" >> "$log"; sleep 10; fi
  ps -axo pid,ppid,pgid,user,stat,wchan,etime,command > "$out/ps-after.txt"
  cp "$hb" "$out/heartbeat.txt"
  ( cd "$(dirname "$out")" && tar czf "/tmp/$label.tar.gz" "$(basename "$out")" )
  gh release upload "$tag" "/tmp/$label.tar.gz" --clobber -R "$GITHUB_REPOSITORY" >> "$log" 2>&1
  summary=$(printf 'dump capture %s (run %s)\n```\n%s\n--- runner-user processes after stall ---\n%s\n```' "$label" "$GITHUB_RUN_ID" "$(cat "$log")" "$(grep -E ' runner ' "$out/ps-before.txt" | head -60)")
  gh api "repos/$GITHUB_REPOSITORY/issues/5926/comments" -f body="$summary" > /dev/null 2>&1
  exit 0
done
