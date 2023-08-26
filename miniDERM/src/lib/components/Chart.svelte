<script lang="ts">
  import { onMount } from "svelte";
  import Chart from "chart.js/auto";
  import "chartjs-adapter-dayjs-4/dist/chartjs-adapter-dayjs-4.esm";
  import { dashboard } from "$lib/store";
  import { derived } from "svelte/store";

  let chart: Chart<"line", { x: string; y: number }[]>;

  const combinedData = derived(dashboard, ($dashboard) => {
    return {
      energyHistory: $dashboard.energyHistory,
      outputHistory: $dashboard.outputHistory,
    };
  });

  let data = {
    datasets: [
      {
        borderColor: "#43A047",
        backgroundColor: "#81C784",
        label: "Energy Generation",
        data: [],
      },
      {
        borderColor: "#1E88E5",
        backgroundColor: "#64B5F6",
        label: "Energy Output",
        data: [],
      },
    ],
  };

  onMount(() => {
    const canvas = document.getElementById("myChart") as HTMLCanvasElement;
    const ctx = canvas.getContext("2d");
    if (!ctx) return;
    chart = new Chart(ctx, {
      type: "line",
      data: {
        datasets: [
          {
            borderColor: "#43A047",
            backgroundColor: "#81C784",
            label: "Energy Generation",
            data: [],
            tension: 0.5
          },
          {
            borderColor: "#1E88E5",
            backgroundColor: "#64B5F6",
            label: "Energy Output",
            data: [],
            tension: 0.5
          },
        ],
      },
      options: {
        scales: {
          y: {
            min: 0,
          },
          x: {
            type: "time",
            time: {
              unit: "minute",
            },
          },
        },
      },
    });
  });

  $: if (chart && $combinedData) {
    chart.data.datasets[0].data = $combinedData.energyHistory.map(value => ({ x: value.Time, y: value.Amount }));
    chart.data.datasets[1].data = $combinedData.outputHistory.map(value => ({ x: value.Time, y: value.Amount }));
    chart.update();
  }
</script>

<canvas id="myChart" />

<style>
  canvas {
    max-height: 200px;
  }
</style>
