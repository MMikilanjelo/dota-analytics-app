import React from "react";
import {Text, TouchableOpacity, GestureResponderEvent} from "react-native";

type TextButtonProps = {
    title: string;
    onPress: (event: GestureResponderEvent) => void;
    className?: string;
};

export default function TextButton({title, onPress, className}: TextButtonProps) {
    return (
        <TouchableOpacity onPress={onPress} activeOpacity={0.7}>
            <Text className={`text-base text-black underline font-inter ${className ?? ""}`}>
                {title}
            </Text>
        </TouchableOpacity>
    );
}