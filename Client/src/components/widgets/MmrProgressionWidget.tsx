import React from "react";
import {View, Text} from "react-native";
import {CartesianChart, Line} from "victory-native";

const MmrProgressionWidget = () => {
    const data = [
        {day: "Mon", mmr: 4200},
        {day: "Tue", mmr: 4230},
        {day: "Wed", mmr: 4300},
        {day: "Thu", mmr: 4290},
        {day: "Fri", mmr: 4350},
    ];

    return (
        <View className="bg-white rounded-2xl p-5 shadow-md h-80 border border-black">
            <Text className="text-black text-xl font-bold mb-4">
                MMR Progression
            </Text>
            <CartesianChart
                data={data}
                xKey="day"
                yKeys={["mmr"]}
                domainPadding={{left: 20, right: 20, top: 20, bottom: 20}}
            >
                {({points}) => (
                    <Line
                        points={points.mmr}
                        color="#2563eb"    // blue
                        strokeWidth={3}
                    />
                )}
            </CartesianChart>
            <Text className="text-gray-500 text-sm mt-3 text-center">
                This week’s rank climb
            </Text>
        </View>
    );
};

export default MmrProgressionWidget;
