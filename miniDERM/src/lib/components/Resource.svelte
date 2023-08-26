<script lang="ts">
  import {
    connectResourceToGrid,
    deleteResource,
    disconnectResourceFromGrid,
  } from "$lib/api";
  import type { EnergyResource } from "$lib/types";
  import EditForm from "./EditForm.svelte";

  export let resource: EnergyResource;
  let isEditing = false;

  const handleDeleteResource = async () => {
    try {
      await deleteResource(resource.Owner, resource.Id);
    } catch (error) {
      console.log("Unable to remove resource");
    }
  };

  const handleConnectToGrid = async () => {
    try {
      await connectResourceToGrid(resource.Owner, resource.Id);
    } catch (error) {
      console.log("Unable to connect resource to grid");
    }
  };

  const handleDisconnectFromGrid = async () => {
    try {
      await disconnectResourceFromGrid(resource.Owner, resource.Id);
    } catch (error) {
      console.log("Unable to connect resource to grid");
    }
  };

  const handleSave = async () => {};
</script>

<div class="resource">
  <div class="resource-header">
    <h3>{resource.Name}</h3>
    <span class="nowrap">
      {#if resource.IsConnectedToGrid}
        <span title="Connected to Grid">🔌</span>
      {/if}
      <button
        class="delete-resource"
        title="Edit"
        on:click={() => {
          isEditing = true;
        }}>🖊️</button
      >
      <button
        class="delete-resource"
        title="Delete"
        on:click={handleDeleteResource}>❌</button
      >
    </span>
  </div>
  {#if isEditing}
    <EditForm resource={resource} onClose={() => { isEditing = false }} />
  {:else}
    <div class="stat-container">
        <span>
        <h5>Status</h5>
        <p>{resource.Status}</p>
        </span>
        <span>
        <h5>Generating</h5>
        <p>{resource.EnergyGeneration}kW</p>
        </span>
        <span>
        <h5>Current Output</h5>
        <p>{resource.EnergyOutput}%</p>
        </span>
        <span>
        <button
            on:click={resource.IsConnectedToGrid
            ? handleDisconnectFromGrid
            : handleConnectToGrid}
            >{resource.IsConnectedToGrid
            ? "Disconnect from Grid"
            : "Connect to Grid"}</button
        >
        </span>
    </div>
  {/if}
</div>

<style>
  .resource {
    background: white;
    border: 2px solid rgb(185, 185, 185);
    border-radius: 16px;
    padding: 2em;
    box-shadow: 0px 10px 15px -3px rgba(0, 0, 0, 0.1);
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

  h3,
  h5,
  p {
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
