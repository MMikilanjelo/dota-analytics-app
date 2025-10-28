import React from 'react'
import {KeyboardAvoidingView, Platform, ScrollView} from 'react-native'
import {Redirect, Slot} from "expo-router";
import {useAuthStore} from "@/src/store/authStore";

export default function AuthLayout() {

    const {isAuthenticated} = useAuthStore();

    if (isAuthenticated) {
        return <Redirect href="/"/>
    }

    return (
        <KeyboardAvoidingView behavior={Platform.OS === 'ios' ? 'padding' : 'height'}>
            <ScrollView className="bg-white h-full" keyboardShouldPersistTaps="handled">
                <Slot/>
            </ScrollView>
        </KeyboardAvoidingView>
    )
}

