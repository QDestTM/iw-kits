import axios from 'axios';

const API_URL = 'http://localhost:23000/api/v1/orders';

export interface TaxJurisdiction {
  name: string;
  type: string;
  rate: number;
}

export interface Order {
  id: string;
  latitude: number;
  longitude: number;
  subtotal: number;
  composite_tax_rate: number;
  tax_amount: number;
  total_amount: number;
  breakdown: {
    state_rate: number;
    country_rate: number;
    city_rate: number;
    special_rate: number;
  };
  jurisdictions: TaxJurisdiction[];
  timestamp: string;
}

export interface CreateOrderDto {
  latitude: number;
  longitude: number;
  subtotal: number;
}

export const ordersApi = {
  getAll: async (): Promise<Order[]> => {
    const response = await axios.get(API_URL);
    return response.data?.items || [];
  },

  create: async (data: CreateOrderDto): Promise<Order> => {
    const response = await axios.post(API_URL, data);
    return response.data?.created_order;
  },

  getImportUrl: (): string => {
    return `${API_URL}/import`;
  }
};
