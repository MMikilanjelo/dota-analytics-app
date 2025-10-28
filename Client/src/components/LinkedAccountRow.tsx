import React from "react";
import {View, Text, TouchableOpacity, Pressable} from "react-native";

interface LinkedAccountRowProps {
    icon: React.ReactNode;
    name: string;
    linkedId?: string | null;
    onLink: () => void;
    onUnlink: () => void;
}

const LinkedAccountRow: React.FC<LinkedAccountRowProps> = (
    {
        icon,
        name,
        linkedId,
        onLink,
        onUnlink,
    }) => {
    const isLinked = !!linkedId;

    return (
        <View className="flex-row items-center justify-between bg-gray-50 rounded-xl p-4 mb-3 border border-gray-200">
            <View className="flex-row items-center">
                {icon}
                <View className="ml-3">
                    <Text className="text-lg font-semibold text-black">{name}</Text>
                    {isLinked ? (
                        <Text className="text-gray-500 text-sm">Linked: {linkedId}</Text>
                    ) : (
                        <Text className="text-gray-400 text-sm">Not linked</Text>
                    )}
                </View>
            </View>

            {isLinked ? (
                <Pressable
                    onPress={onUnlink}
                    className="bg-red-500 px-4 py-2 rounded-lg"
                >
                    <Text className="text-white font-semibold text-sm">Unlink</Text>
                </Pressable>
            ) : (
                <Pressable
                    onPress={onLink}
                    className="bg-blue-600 px-4 py-2 rounded-lg"
                >
                    <Text className="text-white font-semibold text-sm">Link</Text>
                </Pressable>
            )}
        </View>
    );
};

export default LinkedAccountRow;
