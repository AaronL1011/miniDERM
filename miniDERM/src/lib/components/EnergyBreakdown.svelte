<script>

  import { dashboard } from "$lib/store";

</script>
<section class="energy-stats">
    <div>
        <h3>Current Output</h3>
        <p>{$dashboard.currentOutput} kW/h</p>
    </div>
    <div>
        <h3>Total Generation</h3>
        <p>{$dashboard.totalGeneration} kW/h</p>
    </div>
</section>
<style>
    .energy-stats {
        display: flex;
        gap: 2rem;
        background: white;
        border: 2px solid rgb(185, 185, 185);
        border-radius: 16px;
        padding: 2em;
        box-shadow: 0px 10px 15px -3px rgba(0,0,0,0.1);
    }
    h3 {
        margin: 0;
    }
</style>