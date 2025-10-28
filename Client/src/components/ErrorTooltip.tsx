import React, {useEffect} from "react";
import {Text} from "react-native";
import {Ionicons} from "@expo/vector-icons";
import Animated, {
    useAnimatedStyle,
    useSharedValue,
    withTiming,
} from "react-native-reanimated";

type ErrorTooltipProps = {
    error?: string;
};

export default function ErrorTooltip({error}: ErrorTooltipProps) {
    const scale = useSharedValue(0);

    useEffect(() => {
        if (error) {
            scale.value = withTiming(1.1, {duration: 200}, () => {
                scale.value = withTiming(1, {duration: 80});
            });
        } else {
            scale.value = withTiming(0, {duration: 120});
        }
    }, [error]);

    const animatedStyle = useAnimatedStyle(() => ({
        transform: [{scale: scale.value}],
        opacity: scale.value,
    }));

    if (!error) return null;

    return (
        <Animated.View
            style={[animatedStyle]}
            className="flex-row items-center gap-1"
        >
            <Ionicons name="alert-circle-outline" size={14} color="red"/>
            <Text className="text-sm text-red-600">{error}</Text>
        </Animated.View>
    );
}
