<script lang="ts">
  import { deleteResource } from "$lib/api";
    import type { EnergyResource } from "$lib/types";

    export let resource: EnergyResource;

    const handleDeleteResource = async () => {
        try {
            await deleteResource(resource.Owner, resource.Id);
        } catch (error) {
            console.log("Failed to remove resource");
        }
    }

</script>
<div class="resource">
    <div class="resource-header">
        <h3>{resource.Name}</h3>
        <span class="nowrap">
            <button class="delete-resource" title="Edit">🖊️</button>
            <button class="delete-resource" title="Delete" on:click={handleDeleteResource}>❌</button>
        </span>
    </div>
    <div class="stat-container">
        <span>
            <h5>Status</h5>
            <p>{resource.Status}</p>
        </span>
        <span>
            <h5>Generating</h5>
            <p>{resource.EnergyGeneration}kW/h</p>
        </span>
        <span>
            <h5>Current Output</h5>
            <p>{resource.EnergyOutput}%</p>
        </span>
        <span>
            <h5>Connected to Grid</h5>
            <p>{resource.IsConnectedToGrid ? "Yes" : "No"}</p>
        </span>
    </div>
</div>

<style>
    .resource {
        background: white;
        border: 2px solid rgb(185, 185, 185);
        border-radius: 16px;
        padding: 2em;
        box-shadow: 0px 10px 15px -3px rgba(0,0,0,0.1);
        width: 300px;
    }

    .resource-header {
        display: flex;
        justify-content: space-between;
        gap: 1rem;
    }

    .delete-resource {
        border: none;
        background: none;
    }

    h3, h5, p {
        margin: 0;
        margin-bottom: 1em;
    }

    .nowrap {
        min-width: fit-content;
    }

    .stat-container {
        display: flex;
        flex-wrap: wrap;
        width: 100%;
        gap: 1rem;
    }
</style>