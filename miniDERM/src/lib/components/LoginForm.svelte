<script lang="ts">
    import { session } from "$lib/store";
    
    let newOperatorName = $session.operatorName;

    const handleOperatorNameUpdate = (e: Event) => {
        const input = e.target as HTMLInputElement;
        newOperatorName = input.value;
    }

    const login = () => {
        if (newOperatorName.length === 0) {
            return;
        }

        $session.operatorName = newOperatorName;
        $session.isLoggedIn = true;
    }
</script>

<div class="form-container">
    <h1>Welcome</h1>
    <p>Log in to continue to miniDERM</p>
    <div>
        <label for="operator">Operator</label>
        <input id="operator" type="text" placeholder="Enter your Operator Name" value={newOperatorName} on:input={handleOperatorNameUpdate}>
        <br/>
        <br/>
        <button on:click={login}>Enter</button>
    </div>
</div>

<style>
    h1, p {
        margin: 0;
    }

    label {
        display: block;
        font-family: 'IBM Plex Sans';
        margin-bottom: 0.5em;
    }

    .form-container {
        display: flex;
        flex-direction: column;
        gap: 2rem;
        align-items: center;
        justify-content: flex-start;
        width: 300px;
        height: 400px;
        margin: 3rem auto;
        background: white;
        border: 2px solid rgb(185, 185, 185);
        border-radius: 16px;
        padding: 1em;
        box-shadow: 0px 10px 15px -3px rgba(0,0,0,0.1);
    }
</style>