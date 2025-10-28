import React from "react";
import {Modal, View, Text, TouchableOpacity} from "react-native";
import PrimaryButton, {PrimaryButtonProps} from "@/src/components/PrimaryButton";

type MessageModalProps = {
    visible: boolean;
    title?: string;
    message: string;
    buttonProps: PrimaryButtonProps;
    onRequestClose?: () => void;
};

export default function MessageModal(
    {
        visible,
        title,
        message,
        buttonProps,
        onRequestClose
    }: MessageModalProps) {
    return (
        <Modal
            visible={visible}
            transparent={true}
            animationType="slide"
            onRequestClose={onRequestClose}
        >
            <View className="flex-1 justify-center items-center ">
                <View className="w-80 bg-white rounded-2xl p-5 shadow-lg">
                    {title && (
                        <Text className="text-xl font-bold mb-3 text-center text-black">
                            {title}
                        </Text>
                    )}

                    <Text className="text-base text-black text-center">{message}</Text>
                    <PrimaryButton
                        {...buttonProps}
                    />
                </View>
            </View>
        </Modal>
    );
}
