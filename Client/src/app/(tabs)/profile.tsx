import React from "react";
import {View, Text, TouchableOpacity} from "react-native";
import {useSteamLogin} from "@/src/hooks/useSteamLogin";
import {Gamepad2, MessageCircle, User} from "lucide-react-native";
import PrimaryButton from "@/src/components/PrimaryButton";
import LinkedAccountRow from "@/src/components/LinkedAccountRow";

export default function Profile() {
    const {steamId, status, login} = useSteamLogin("(tabs)/profile");

    return (
        <View className="flex-1 bg-white p-6 gap-6">
            <View className="flex items-center justify-center pt-6">
                <User size={80} color="black" strokeWidth={1.5}/>
                <Text className="text-2xl font-bold text-black mt-4">Player123</Text>
                <Text className="text-gray-500">player123@email.com</Text>
            </View>

            <View className="border border-black p-4 rounded-2xl bg-white">
                <Text className="text-xl font-bold text-black mb-4">
                    Linked Accounts
                </Text>

                <LinkedAccountRow
                    icon={<Gamepad2 size={28} color="black"/>}
                    name="Steam"
                    linkedId={steamId}
                    onLink={() => console.log("Link Steam")}
                    onUnlink={() => console.log("Unlink Steam")}
                />

                <LinkedAccountRow
                    icon={<MessageCircle size={28} color="black"/>}
                    name="Discord"
                    linkedId={null}
                    onLink={() => console.log("Link Discord")}
                    onUnlink={() => console.log("Unlink Discord")}
                />
            </View>

            {/* Actions */}
            <View className="flex gap-3">
                <PrimaryButton title={"EditProfile"} onPress={() => {
                }}></PrimaryButton>
                <PrimaryButton title={"Log Out"} onPress={() => {
                }}></PrimaryButton>
            </View>
        </View>
    );
}
