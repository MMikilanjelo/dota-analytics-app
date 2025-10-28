import React from "react";
import {View, Text} from "react-native";
import {Pie, PolarChart} from "victory-native";

const RadiantDireWidget = () => {
    const DATA = [
        {label: "Radiant", value: 55, color: "#22c55e"},
        {label: "Dire", value: 45, color: "#ef4444"},
    ];

    return (
        <View className="bg-white rounded-2xl p-5 shadow-md h-80 border border-black">
            <Text className="text-black text-xl font-bold mb-4">Radiant vs Dire</Text>
            <PolarChart
                data={DATA}
                labelKey={"label"}
                valueKey={"value"}
                colorKey={"color"}
            >
                <Pie.Chart innerRadius={50}/>
            </PolarChart>
            <Text className="text-gray-500 text-sm mt-3">Winrate distribution</Text>
        </View>
    );
};

export default RadiantDireWidget;
