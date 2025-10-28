import React from "react";
import {Pressable, Text, View, ActivityIndicator} from "react-native";

export type PrimaryButtonProps = {
    title: string;
    isLoading?: boolean;
    onPress: () => void;
};

export default function PrimaryButton({title, isLoading, onPress}: PrimaryButtonProps) {
    return (
        <View className="flex w-full justify-center items-center">
            <Pressable
                onPress={onPress}
                disabled={isLoading}
                className={`w-full rounded-[10px] h-14 flex-row items-center justify-center bg-black active:opacity-80`}
            >
                {isLoading ? (
                    <ActivityIndicator size="small" color="#fff"/>
                ) : (
                    <Text className="text-white text-lg font-inter-semi-bold">{title}</Text>
                )}
            </Pressable>

        </View>
    );
}
