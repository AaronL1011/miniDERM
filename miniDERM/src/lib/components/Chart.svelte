<script lang="ts">
  import { onMount } from "svelte";
  import Chart from "chart.js/auto";
  import 'chartjs-adapter-dayjs-4/dist/chartjs-adapter-dayjs-4.esm';
  import type { ChartData, ChartOptions } from "chart.js/auto";
  import { dashboard } from "$lib/store";

  let chart: Chart<"line", {x: string, y: number}[]>;

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
  }; // Define the type for data
  let options: ChartOptions = {
    animation: false,
    scales: {
      y: {
        min: 0,
      },
      x: {
        type: "time",
        time: {
            unit: "minute"
        }
      }
    },
  }; // Define the type for options

  onMount(() => {
    const canvas = document.getElementById("myChart") as HTMLCanvasElement;
    const ctx = canvas.getContext("2d");
    if (ctx) {
      dashboard.subscribe((dash) => {
        if (chart) {
          (chart).data = {
            datasets: [
              {
                borderColor: "#43A047",
                backgroundColor: "#81C784",
                label: "Energy Generation",
                data: dash.energyHistory.map((value) => ({
                  x: value.Time,
                  y: value.Amount,
                })),
              },
            ],
          };
          chart.update(); // Update the chart with new data
        } else {
          chart = new Chart<"line", {x: string, y: number}[]>(ctx, {
            type: "line",
            data: data,
            options: options,
          });
        }
      });
    }
  });
</script>

<canvas id="myChart" />

<style>
  canvas {
    max-height: 200px;
  }
</style>
