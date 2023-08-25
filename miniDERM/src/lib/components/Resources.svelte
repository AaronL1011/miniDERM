<script lang="ts">
  import Resource from "./Resource.svelte";
  import Modal from './Modal.svelte';
  import { createResource, getAllResourceInfo } from "$lib/api";
  import { dashboard, session } from "$lib/store";
  import type { EnergyResource } from "$lib/types";
  import { onMount } from "svelte";

	let showModal = false;
    $: resources = $dashboard.resources;

    const handleResourceCreate = async (e: SubmitEvent) => {
        e.preventDefault()
        const form = e.target as HTMLFormElement;
        const formData = new FormData(form);

        // Access form values using the name attributes of the form elements
        const resourceName = formData.get('resourceName');
        const energyOutput = formData.get('energyOutput');
        try {
            await createResource($session.operatorName, { name: resourceName, energyOutput })
        } catch (error) {
            console.log(error);
        }

        showModal = false;
    }
</script>

<section class="resources">
    {#each resources as resource}
        <Resource bind:resource />
    {/each}
    <button class="add-resource" on:click={() => (showModal = true)}>
        <h3>Add Resource</h3>
        <p>+</p>
    </button>
    <Modal bind:showModal>
        <h2 slot="header">
            Add Resource
        </h2>
    
        <form on:submit={handleResourceCreate}>
            <label for="resourceName">Resource Name</label>
            <input type="text" id="resourceName" name="resourceName"/>
            <label for="energyOutput">Output %</label>
            <input type="number" id="energyOutput" name="energyOutput"/>
            <button type="submit">Create</button>
        </form>
    </Modal>
</section>
<style>
    .resources {
        display: flex;
        flex-wrap: wrap;
        gap: 2rem;
    }

    h3 {
        margin: 0;
    }

    .add-resource {
        background: transparent;
        border: 2px solid rgb(185, 185, 185);
        border-radius: 16px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        padding: 2em 4em;
        color: rgb(121, 121, 121);
    }

    .add-resource p {
        font-size: 2em;
        margin: 0;
    }

    .add-resource:hover {
        cursor: pointer;
        color: rgb(46, 46, 46);
        border: 2px solid rgb(46, 46, 46);
    }

    form {
        display: flex;
        flex-direction: column;
        gap: 0.5em;
    }
</style>