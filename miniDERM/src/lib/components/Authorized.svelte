<script lang="ts">
  import LoginForm from "./LoginForm.svelte";

    export let isLoggedIn: boolean;
</script>

<main>
    {#if isLoggedIn}
        <slot/>
    {:else}
        <LoginForm />
    {/if}
</main>

<style>
    main {
        padding: 2rem;
    }
</style>