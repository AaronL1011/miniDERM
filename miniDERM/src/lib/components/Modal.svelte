<script lang="ts">
	export let showModal: boolean;

	let dialog: HTMLDialogElement;

	$: if (dialog && showModal) dialog.showModal();
	$: if (dialog && !showModal) dialog.close();
</script>

<!-- svelte-ignore a11y-click-events-have-key-events a11y-no-noninteractive-element-interactions -->
<dialog
	bind:this={dialog}
	on:close={() => (showModal = false)}
	on:click|self={() => dialog.close()}
>
	<!-- svelte-ignore a11y-no-static-element-interactions -->
	<div class="content" on:click|stopPropagation>
		<slot name="header" />
		<slot />
		<button on:click={() => dialog.close()}>Cancel</button>
	</div>
</dialog>

<style>
	dialog {
		max-width: 32em;
		border: 2px solid rgb(185, 185, 185);
        border-radius: 16px;
        padding: 1em;
        box-shadow: 0px 10px 15px -3px rgba(0,0,0,0.1);
	}
	dialog::backdrop {
		background: rgba(0, 0, 0, 0.3);
	}
	dialog > div {
		padding: 1em;
	}
	dialog[open] {
		animation: zoom 0.3s cubic-bezier(0.34, 1.56, 0.64, 1);
	}
	@keyframes zoom {
		from {
			transform: scale(0.95);
		}
		to {
			transform: scale(1);
		}
	}
	dialog[open]::backdrop {
		animation: fade 0.2s ease-out;
	}
	@keyframes fade {
		from {
			opacity: 0;
		}
		to {
			opacity: 1;
		}
	}
	button {
        margin-top: 1em;
		display: inline;
        width: max-content;
	}
    
    .content {
        display:flex;
        flex-direction: column;
    }
</style>
