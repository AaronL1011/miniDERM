<script lang="ts">
  import EnergyBreakdown from "$lib/components/EnergyBreakdown.svelte";
  import Resources from "$lib/components/Resources.svelte";
  import { dashboard, session } from "$lib/store";
  import type { EnergyResourcesInfo } from "$lib/types";
  import { onMount } from "svelte";

  onMount(() => {
    const socket = new WebSocket(`ws://localhost:8080/Operator/${$session.operatorName}/ws`);

    socket.addEventListener("message", (event) => {
      const newState = JSON.parse(event.data) as EnergyResourcesInfo;
      $dashboard.resources = newState.Resources;
      $dashboard.currentOutput = newState.CurrentOutput;
      $dashboard.totalGeneration = newState.TotalGeneration;
    });

    return () => {
        socket.close()
    }
  });
</script>

<h1>Dashboard</h1>
<h2 class="section-header">Energy Breakdown</h2>
<EnergyBreakdown />
<h2 class="section-header">Resources</h2>
<Resources />

<style>
  h1 {
    margin: 0;
  }

  .section-header {
    color: rgb(68, 68, 68);
  }
</style>
