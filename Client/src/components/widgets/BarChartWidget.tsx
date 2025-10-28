import React from "react";
import {View, Text} from "react-native";
import {Bar, CartesianChart} from "victory-native";
import {LinearGradient, vec} from "@shopify/react-native-skia";

const SkiaBarChartWidget = () => {
    const data = [
        {day: "Mon", value: 3},
        {day: "Tue", value: 5},
        {day: "Wed", value: 2},
        {day: "Thu", value: 7},
        {day: "Fri", value: 4},
    ];

    return (
        <View className="bg-white rounded-2xl p-5 shadow-md h-80 border border-black">
            <Text className="text-black text-xl font-bold mb-4">
                Winrate Trend
            </Text>
            <CartesianChart
                data={data}
                xKey="day"
                yKeys={["value"]}
                domainPadding={{left: 25, right: 25, top: 20, bottom: 20}}
            >
                {({points, chartBounds}) => (
                    <Bar
                        chartBounds={chartBounds}
                        points={points.value}
                        roundedCorners={{topLeft: 8, topRight: 8}}
                    >
                        <LinearGradient
                            start={vec(0, 0)}
                            end={vec(0, 300)}
                            colors={["#3b82f6", "#9333ea"]} // blue → purple
                        />
                    </Bar>
                )}
            </CartesianChart>
            <Text className="text-gray-500 text-sm mt-3 text-center">
                Matches played this week
            </Text>
        </View>
    );
};

export default SkiaBarChartWidget;
