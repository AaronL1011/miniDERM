<script lang="ts">
  import { setEnergyOutput } from "$lib/api";
  import type { EnergyResource } from "$lib/types";

    export let resource: EnergyResource
    export let onClose: () => void;

    let name = resource.Name;
    let output = resource.EnergyOutput;
    let status = resource.Status

    const handleNameUpdate = (e: Event) => {
        const input = e.target as HTMLInputElement;
        name = input.value;
    }
    const handleOutputUpdate = (e: Event) => {
        const input = e.target as HTMLInputElement;
        output = Number(input.value);
    }
    const handleStatusUpdate = (e: Event) => {
        const input = e.target as HTMLInputElement;
        status = input.value;
    }

    const saveOutput = async () => {
        if (output === resource.EnergyOutput) return;

        try {
            await setEnergyOutput(resource.Owner, { id: resource.Id, energyOutput: output })
        } catch (error) {
            console.error("Unable to set Energy Output")
        }    
    }
</script>

<div class="edit-form">
    <span class="resource-field">
        <label for="name">Name</label>
        <input type="text" value={name} on:input={handleNameUpdate} /><button>Save</button>
    </span>
    <span class="resource-field">
        <label for="output">Output</label>
        <input id="output" type="number" max="100" min="0" value={output} on:input={handleOutputUpdate}/><button on:click={saveOutput}>Save</button>
    </span>
    <span class="resource-field">
        <label for="status">Status</label>
        <select id="status" value={status} on:select={handleStatusUpdate}>
            <option value="Active">Active</option>
            <option value="Inactive">Inactive</option>
        </select>
        <button>Save</button>
    </span>
    <button on:click={onClose}>Cancel</button>
</div>

<style>
    .edit-form { 
        display: flex;
        flex-direction: column;
        gap: 1em;
    }

    .edit-form button {
        margin-left: 0.5em;
    }

    input[type="text"] {
        width: 150px;
    }

    input[type="number"] {
        width: 50px;
    }
</style>