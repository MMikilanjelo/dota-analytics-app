import {create} from "zustand";
import {persist} from "zustand/middleware";

type User = {
    id: string;
    email: string;
};

type AuthState = {
    user: User | null;
    accessToken: string | null;
    refreshToken: string | null;
    isAuthenticated: boolean;
    setAuth: (user: User, accessToken: string, refreshToken: string) => void;
    setTokens: (accessTokens: string, refreshTokens: string) => void;
    clearAuth: () => void;
};

export const useAuthStore = create<AuthState>()(
    (set) => ({
        user: null,
        accessToken: null,
        refreshToken: null,
        isAuthenticated: true,
        hasHydrated: false,
        setAuth: (user, accessToken, refreshToken) => set({user, accessToken, refreshToken, isAuthenticated: true}),
        setTokens: (accessToken: string, refreshToken: string) => set({accessToken, refreshToken, isAuthenticated: true}),
        clearAuth: () => set({user: null, accessToken: null, refreshToken: null, isAuthenticated: false}),
    })
);


