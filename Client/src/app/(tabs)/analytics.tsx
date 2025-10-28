import React from "react";
import {Text, View, ScrollView} from "react-native";
import BarChartWidget from "@/src/components/widgets/BarChartWidget";
import MmrProgressionWidget from "@/src/components/widgets/MmrProgressionWidget";
import RadiantDireWidget from "@/src/components/widgets/RadiantDireWidget";
import HeroWinrateWidget from "@/src/components/widgets/HeroWinrateWidget";

function Analytics() {
    return (
        <ScrollView className="flex-1 bg-gray-50">
            <View className="p-4 gap-4">
                <HeroWinrateWidget/>
                <BarChartWidget/>
                <MmrProgressionWidget/>
                <RadiantDireWidget/>

                <View className="bg-white rounded-xl p-4 shadow mt-4">
                    <Text className="text-lg font-semibold">More insights coming soon...</Text>
                </View>
            </View>
        </ScrollView>
    );
}

export default Analytics;

