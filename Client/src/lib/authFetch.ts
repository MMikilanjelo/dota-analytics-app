import {useAuthStore} from "@/src/store/authStore";
import RefreshAccessTokenMutationRequest from "@/src/graphql/mutations/__generated__/RefreshAccessTokenMutation.graphql";

const GRAPHQL_ENDPOINT = "http://192.168.0.191:5095/graphql";

let refreshPromise: Promise<string | null> | null = null;

async function refreshAccessToken(refreshToken: string): Promise<string | null> {
    if (!refreshPromise) {
        refreshPromise = (async () => {
            const res = await fetch(GRAPHQL_ENDPOINT, {
                method: "POST",
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify({
                    query: RefreshAccessTokenMutationRequest.params.text,
                    variables: {input: {refreshToken}},
                }),
            });

            const json = await res.json();
            const newAccess = json.data?.refreshAccessToken?.accessToken;
            const newRefresh = json.data?.refreshAccessToken?.refreshToken;

            if (newAccess && newRefresh) {
                useAuthStore.getState().setTokens(newAccess, newRefresh);
                return newAccess;
            }

            useAuthStore.getState().clearAuth();
            return null;
        })();

        refreshPromise.finally(() => {
            refreshPromise = null;
        });
    }
    return refreshPromise;
}

export async function fetchWithAuthRetry(
    url: string,
    options: RequestInit = {}
): Promise<Response> {
    const {accessToken, refreshToken} = useAuthStore.getState();

    const headers: HeadersInit = {
        ...(options.headers || {}),
        ...(accessToken ? {Authorization: `Bearer ${accessToken}`} : {}),
    };

    let response = await fetch(url, {...options, headers});

    if (response.status === 401 && refreshToken) {
        const newAccess = await refreshAccessToken(refreshToken);
        if (newAccess) {
            const retryHeaders: HeadersInit = {
                ...(options.headers || {}),
                Authorization: `Bearer ${newAccess}`,
            };
            response = await fetch(url, {...options, headers: retryHeaders});
        }
    }

    return response;
}

export async function fetchGraphQL(query: string, variables: any): Promise<any> {
    const res = await fetchWithAuthRetry(GRAPHQL_ENDPOINT, {
        method: "POST",
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify({query, variables}),
    });
    return res.json();
}
