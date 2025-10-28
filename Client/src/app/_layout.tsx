import 'react-native-reanimated';
import {useEffect} from "react";
import {SplashScreen, Stack} from "expo-router";
import {useFonts} from "expo-font";
import './globals.css';
import SafeScreen from "@/src/components/SafeScreen";
import RelayEnvironment from "@/src/lib/relay/RelayEnvironment";
import {ModalProvider} from "@/src/contexts/ModalContext";

export default function RootLayout() {
    const [fontsLoaded] = useFonts({
        "Inter-Regular": require("@/assets/fonts/inter/Inter-Regular.ttf"),
        "Poppins-Bold": require("@/assets/fonts/poppins/Poppins-Bold.ttf"),
        "Inter-SemiBold": require("@/assets/fonts/inter/Inter-SemiBold.ttf"),
    });

    useEffect(() => {
        if (fontsLoaded) {
            SplashScreen.hideAsync();
        }
    }, [fontsLoaded]);

    if (!fontsLoaded) {
        return null;
    }

    return (
        <RelayEnvironment>
            <SafeScreen>
                <ModalProvider>
                    <Stack
                        screenOptions={{
                            headerShown: false,
                            animation: "slide_from_right",
                            animationDuration: 150,
                        }}
                    />
                </ModalProvider>
            </SafeScreen>
        </RelayEnvironment>
    );
}
