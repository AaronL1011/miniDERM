import { writable } from 'svelte/store';
import { DEFAULT_OPERATOR_NAME } from './constants';
import type { EnergyResource, EnergyTimestamp } from './types';
 
export const session = writable({
    operatorName: DEFAULT_OPERATOR_NAME,
    isLoggedIn: false
});

interface DashboardStore {
    resources: EnergyResource[],
    energyHistory: EnergyTimestamp[],
    outputHistory: EnergyTimestamp[],
    currentOutput: number,
    totalGeneration: number
}

export const dashboard = writable<DashboardStore>({
    resources: [],
    energyHistory: [],
    outputHistory: [],
    currentOutput: 0,
    totalGeneration: 0,
})