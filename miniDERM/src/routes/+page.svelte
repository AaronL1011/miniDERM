<script lang="ts">
  import EnergyBreakdown from "$lib/components/EnergyBreakdown.svelte";
  import Resources from "$lib/components/Resources.svelte";
  import { dashboard, session } from "$lib/store";
  import type { EnergyResourcesInfo } from "$lib/types";
  import { onMount } from "svelte";

  onMount(() => {
    const socket = new WebSocket(`ws://localhost:6001/Operator/${$session.operatorName}/ws`);

    socket.addEventListener("message", (event) => {
      const newState = JSON.parse(event.data) as EnergyResourcesInfo;
      $dashboard.resources = newState.Resources;
      $dashboard.energyHistory = newState.EnergyHistory;
      $dashboard.outputHistory = newState.OutputHistory;
      $dashboard.currentOutput = newState.CurrentOutput;
      $dashboard.totalGeneration = newState.TotalGeneration;
    });

    return () => {
        socket.close()
    }
  });
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
