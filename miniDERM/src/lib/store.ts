import { writable } from 'svelte/store';
import { DEFAULT_OPERATOR_NAME } from './constants';
import type { EnergyResource } from './types';
 
export const session = writable({
    operatorName: DEFAULT_OPERATOR_NAME,
    isLoggedIn: false
});

interface DashboardStore {
    resources: EnergyResource[],
    currentOutput: number,
    totalGeneration: number
}

export const dashboard = writable<DashboardStore>({
    resources: [],
    currentOutput: 0,
    totalGeneration: 0
})