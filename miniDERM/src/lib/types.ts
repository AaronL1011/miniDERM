export type CreateEnergyResource = {
    name: string;
    energyOutput: number;
}

export type SetEnergyOutput = {
    id: string;
    energyOutput: number;
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
export interface EnergyResourcesInfo {
    Resources: EnergyResource[];
    CurrentOutput: number;
    TotalGeneration: number;
}