import React, {useEffect} from "react";
import {Pressable, Text} from "react-native";
import Animated, {
    interpolate,
    useAnimatedStyle,
    useSharedValue,
    withSpring,
} from "react-native-reanimated";

interface TabBarButtonProps {
    isFocused: boolean;
    label: string;
    colorClass: string;
    onPress: () => void;
    onLongPress?: () => void;
    icon: React.ReactNode;
}

const TabBarButton: React.FC<TabBarButtonProps> = (
    {
        isFocused,
        label,
        colorClass,
        onPress,
        onLongPress,
        icon,
    }) => {
    const scale = useSharedValue(isFocused ? 1 : 0);

    useEffect(() => {
        scale.value = withSpring(isFocused ? 1 : 0, {
            damping: 20,
            stiffness: 200,
            mass: 0.5,
        });
    }, [isFocused]);

    const iconStyle = useAnimatedStyle(() => ({
        transform: [
            {scale: interpolate(scale.value, [0, 1], [1, 1.3])},
            {translateY: interpolate(scale.value, [0, 1], [0, -2])},
        ],
    }));

    return (
        <Pressable
            className="flex-1 justify-center items-center"
            onPress={onPress}
            onLongPress={onLongPress}
        >
            <Animated.View style={iconStyle}>{icon}</Animated.View>
            <Text className={`text-xs mt-1 font-medium ${colorClass}`}>
                {label}
            </Text>
        </Pressable>
    );
};

export default TabBarButton;


