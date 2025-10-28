import {useCallback, useState, useEffect} from "react";
import * as WebBrowser from "expo-web-browser";
import * as Linking from "expo-linking";
import {fetchWithAuthRetry} from "@/src/lib/authFetch";

const BACKEND_URL = "http://192.168.0.191:5095/api/external/steam/login";

export function useSteamLogin(redirectPath: string) {
    const [steamId, setSteamId] = useState<string | null>(null);
    const [status, setStatus] = useState<string>("");

    useEffect(() => {
        const sub = Linking.addEventListener("url", ({url}) => {
            try {
                const parsed = Linking.parse(url);
                const id = parsed.queryParams?.steamId as string | undefined;

                if (id) {
                    setSteamId(id);
                    setStatus("Steam linked successfully!");
                } else {
                    setStatus("Could not extract Steam ID from redirect");
                }
            } catch (err) {
                console.error("Failed to parse redirect:", err);
                setStatus("Error parsing redirect");
            }
        });

        return () => sub.remove();
    }, []);

    const login = useCallback(async () => {
        setStatus("Opening Steam login...");
        try {
            const res = await fetchWithAuthRetry(
                `${BACKEND_URL}?redirect_uri=${encodeURIComponent(
                    Linking.createURL(redirectPath)
                )}`
            );

            if (!res.ok) {
                setStatus(`Steam login failed: ${res.status}`);
                return;
            }

            const {url} = await res.json();

            await WebBrowser.openBrowserAsync(url, {showInRecents: true});
            
        } catch (err) {
            console.error("Steam login error:", err);
            setStatus("Error during login.");
        }
    }, [redirectPath]);

    return {steamId, status, login};
}
