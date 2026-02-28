import React, { createContext, useContext, useState, useEffect, useCallback } from 'react';
import { authApi, type AuthUser } from '../api/auth.api.js';

interface AuthContextType {
    user: AuthUser | null;
    isAuthenticated: boolean;
    loading: boolean;
    login: (username: string, password: string) => Promise<string | null>;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

export function useAuth(): AuthContextType {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within AuthProvider');
    }
    return context;
}

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [user, setUser] = useState<AuthUser | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const storedUser = localStorage.getItem('user');
        const accessToken = localStorage.getItem('access_token');

        if (storedUser && accessToken) {
            const parsedUser = JSON.parse(storedUser) as AuthUser;
            if (parsedUser.role === 'admin') {
                setUser(parsedUser);
            } else {
                localStorage.removeItem('access_token');
                localStorage.removeItem('refresh_token');
                localStorage.removeItem('user');
            }
        }
        setLoading(false);
    }, []);

    const login = useCallback(async (username: string, password: string): Promise<string | null> => {
        try {
            const response = await authApi.login(username, password);

            if (response.error_message) {
                return response.error_message;
            }

            if (!response.user || response.user.role !== 'admin') {
                return 'Only admin users are allowed to access this application';
            }

            if (response.access_token && response.refresh_token && response.user) {
                localStorage.setItem('access_token', response.access_token);
                localStorage.setItem('refresh_token', response.refresh_token);
                localStorage.setItem('user', JSON.stringify(response.user));
                setUser(response.user);
            }

            return null;
        } catch (err: unknown) {
            if (err && typeof err === 'object' && 'response' in err) {
                const axiosErr = err as { response?: { data?: { error_message?: string } } };
                return axiosErr.response?.data?.error_message ?? 'Login failed';
            }
            throw err;
        }
    }, []);

    const logout = useCallback(() => {
        localStorage.removeItem('access_token');
        localStorage.removeItem('refresh_token');
        localStorage.removeItem('user');
        setUser(null);
    }, []);

    return (
        <AuthContext.Provider value={{
            user,
            isAuthenticated: !!user,
            loading,
            login,
            logout,
        }}>
            {children}
        </AuthContext.Provider>
    );
}
