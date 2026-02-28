import axios from 'axios';

const AUTH_URL = '/api/v1/auth';

export interface AuthUser {
    id: string;
    username: string;
    role: string;
}

export interface AuthResponse {
    access_token: string | null;
    refresh_token: string | null;
    user: AuthUser | null;
    error_message: string | null;
}

export interface RefreshResponse {
    access_token: string | null;
    refresh_token: string | null;
    error_message: string | null;
}

export const authApi = {
    login: async (username: string, password: string): Promise<AuthResponse> => {
        const response = await axios.post(`${AUTH_URL}/login`, { username, password });
        return response.data;
    },

    refresh: async (refreshToken: string): Promise<RefreshResponse> => {
        const response = await axios.post(`${AUTH_URL}/refresh`, { refresh_token: refreshToken });
        return response.data;
    },
};
