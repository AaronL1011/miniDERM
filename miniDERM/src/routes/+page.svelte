<script lang="ts">
  import EnergyBreakdown from "$lib/components/EnergyBreakdown.svelte";
  import Resources from "$lib/components/Resources.svelte";
  import { dashboard, session } from "$lib/store";
  import type { EnergyResourcesInfo } from "$lib/types";
  import { onDestroy, onMount } from "svelte";

  let socket: WebSocket;

  const onMessage = (e: MessageEvent<string>) => {
    const newState = JSON.parse(e.data) as EnergyResourcesInfo;
    $dashboard.resources = newState.Resources;
    $dashboard.energyHistory = newState.EnergyHistory;
    $dashboard.outputHistory = newState.OutputHistory;
    $dashboard.currentOutput = newState.CurrentOutput;
    $dashboard.totalGeneration = newState.TotalGeneration;
  }

  onMount(() => {
    const wsHost = window.location.hostname;
    const wsPort = window.location.port;
    const wsUrl = `ws://${wsHost}:${wsPort}/Operator/${$session.operatorName}/ws`;

    socket = new WebSocket(wsUrl);
    socket.addEventListener("message", onMessage);
  });

  onDestroy(() => {
    socket.close()
    socket.removeEventListener("message", onMessage)
  })
</script>

<h2>Dashboard</h2>
<h3 class="section-header">Energy Breakdown</h3>
<EnergyBreakdown />
<h3 class="section-header">Resources</h3>
<Resources />

<style>
  h2 {
    margin: 0;
  }

  .section-header {
    color: rgb(68, 68, 68);
  }
</style>
