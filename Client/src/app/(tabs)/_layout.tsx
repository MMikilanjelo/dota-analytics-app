import React from "react";
import {Redirect, Tabs} from "expo-router";
import {useAuthStore} from "@/src/store/authStore";
import TabBar from "@/src/components/TabBar"; // 👈 import your custom tab bar

export default function HomeLayout() {
    const {isAuthenticated} = useAuthStore();

    if (!isAuthenticated) {
        return <Redirect href={"/(auth)/sign-in"}/>;
    }

    return (
        <Tabs
            tabBar={(props) => <TabBar {...props} />}
            screenOptions={{
                headerShown: false,
            }}
        >
            <Tabs.Screen
                name="index"
                options={{
                    title: "Home",
                }}
            />
            <Tabs.Screen
                name="analytics"
                options={{
                    title: "Analytics",
                }}
            />
            <Tabs.Screen
                name="profile"
                options={{
                    title: "Profile",
                }}
            />
        </Tabs>
    );
}
