<script lang="ts">
    import Authorized from "$lib/components/Authorized.svelte";
    import { DEFAULT_OPERATOR_NAME } from "$lib/constants";
    import { dashboard, session } from "$lib/store";

    const handleLogout = () => {
        $session.operatorName = DEFAULT_OPERATOR_NAME;
        $session.isLoggedIn = false;

        $dashboard.resources = [];
        $dashboard.currentOutput = 0;
        $dashboard.totalGeneration = 0;
    }
</script>
<svelte:head>
    <link rel="icon" href="data:image/svg+xml,<svg xmlns=%22http://www.w3.org/2000/svg%22 viewBox=%220 0 100 100%22><text y=%22.9em%22 font-size=%2290%22>🏭</text></svg>">
    <title>miniDERM</title>
</svelte:head>

<nav>
    <h2>🏭 miniDERM</h2>
    {#if $session.isLoggedIn}
        <div class="avatar-menu">
            <h4>{$session.operatorName}</h4>
            <button class="menu-button" on:click={handleLogout}>Logout</button>
        </div>
    {/if}
</nav>
<Authorized isLoggedIn={$session.isLoggedIn}>
    <slot />
</Authorized>

<style>
    nav {
        display: flex;
        justify-content: space-between;
        align-items: center;
        height: 64px;
        background-color: rgb(36, 47, 56);
        color: white;
        padding: 0 1rem;
    }

    h2, h4 {
        margin: 0;
        padding: 0;
    }

    .avatar-menu {
        display: flex;
        gap: 1rem;
    }
</style>