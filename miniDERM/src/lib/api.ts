import axios from 'axios';
import type { CreateEnergyResource, SetEnergyOutput } from './types';

const operatorApi = axios.create({
  baseURL: 'http://localhost:6001/Operator',
  headers: {
    "Content-Type": "application/json"
  }
});

export const getAllResourceInfo = (operatorName: string) => operatorApi.get(`${operatorName}/getResources`);
export const createResource = (operatorName: string, data: CreateEnergyResource) => operatorApi.post(`${operatorName}/create`, JSON.stringify(data));
export const deleteResource = (operatorName: string, resourceId: string) => operatorApi.delete(`${operatorName}`, { data: JSON.stringify({ id: resourceId })});
export const connectResourceToGrid = (operatorName: string, resourceId: string) => operatorApi.post(`${operatorName}/connect`, JSON.stringify({ id: resourceId }));
export const disconnectResourceFromGrid = (operatorName: string, resourceId: string) => operatorApi.post(`${operatorName}/disconnect`, JSON.stringify({ id: resourceId }));
export const setEnergyOutput = (operatorName: string, data: SetEnergyOutput) => operatorApi.post(`${operatorName}/setOutput`, JSON.stringify(data));

// Add more routes as needed
