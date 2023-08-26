<script>

  import { dashboard } from "$lib/store";
  import Chart from "./Chart.svelte";

</script>
<section class="energy-stats">
    <div class="stat-summary">
        <div>
            <h4>Current Output</h4>
            <p>{$dashboard.currentOutput} kW</p>
        </div>
        <div>
            <h4>Total Generation</h4>
            <p>{$dashboard.totalGeneration} kW</p>
        </div>
    </div>
    <Chart />
</section>
<style>
    .energy-stats {
        display: flex;
        flex-direction: column;
        gap: 2rem;
        background: white;
        border: 2px solid rgb(185, 185, 185);
        border-radius: 16px;
        padding: 2em;
        box-shadow: 0px 10px 15px -3px rgba(0,0,0,0.1);
    }
    .stat-summary {
        display: flex;
        flex-direction: row;
        gap: 1em;
    }
 
    h4 {
        margin: 0;
        display: inline;
    }

    p { 
        margin: 0;
    }

</style>