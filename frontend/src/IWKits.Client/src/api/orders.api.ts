import axios from 'axios';
import { authApi } from './auth.api.js';

const API_URL = '/api/v1/orders';

axios.interceptors.request.use((config) => {
  const token = localStorage.getItem('access_token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

axios.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config as typeof error.config & { _retry?: boolean };

    const url = originalRequest?.url ?? '';
    if (url.startsWith('/api/v1/auth')) {
      return Promise.reject(error);
    }

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      const refreshToken = localStorage.getItem('refresh_token');
      if (refreshToken) {
        try {
          const result = await authApi.refresh(refreshToken);

          if (result.access_token && result.refresh_token) {
            localStorage.setItem('access_token', result.access_token);
            localStorage.setItem('refresh_token', result.refresh_token);
            originalRequest.headers.Authorization = `Bearer ${result.access_token}`;
            return await axios(originalRequest);
          }
        } catch {
        }
      }

      localStorage.removeItem('access_token');
      localStorage.removeItem('refresh_token');
      localStorage.removeItem('user');
      window.location.href = '/auth';
    }

    return Promise.reject(error);
  }
);

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
