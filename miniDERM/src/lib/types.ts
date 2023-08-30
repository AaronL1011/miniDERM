export type CreateEnergyResource = {
    name: string;
    energyOutput: number;
    timeZone: string;
}

export type SetEnergyOutput = {
    id: string;
    energyOutput: number;
}

export type SetResourceName = {
    id: string;
    name: string;
}

export interface EnergyResource {
    Id: string;
    Name: string;
    Status: string;
    EnergyOutput: number;
    IsConnectedToGrid: boolean;
    Owner: string;
    EnergyGeneration: number;
}

export interface EnergyTimestamp {
    Time: string;
    Amount: number;
}

export interface EnergyResourcesInfo {
    Resources: EnergyResource[];
    EnergyHistory: EnergyTimestamp[];
    OutputHistory: EnergyTimestamp[];
    CurrentOutput: number;
    TotalGeneration: number;
}