import axios from 'axios';

const API_URL = '/api/v1/orders';

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
    county_rate: number;
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

export interface OrdersQuery {
  page?: number;
  page_size?: number;
  sort_by?: string;
  descending?: boolean;
  min_total_amount?: number;
  max_total_amount?: number;
  from_date?: string;
  to_date?: string;
}

export interface OrdersResponse {
  items: Order[];
  total_count: number;
  total_pages: number;
}

export const ordersApi = {
  getAll: async (query?: OrdersQuery): Promise<OrdersResponse> => {
    const response = await axios.get(API_URL, { params: query });
    return {
      items: response.data?.items || [],
      total_count: response.data?.total_count || 0,
      total_pages: response.data?.total_pages || 0,
    };
  },

  create: async (data: CreateOrderDto): Promise<Order> => {
    const response = await axios.post(API_URL, data);

    if (response.data?.error_message) {
      throw new Error(response.data.error_message);
    }

    return response.data?.created_order;
  },

  getImportUrl: (): string => {
    return `${API_URL}/import`;
  }
};
