import React from "react";
import {View, Text} from "react-native";
import {Bar, CartesianChart} from "victory-native";
import {LinearGradient, vec} from "@shopify/react-native-skia";

const HeroWinrateWidget = () => {
    const data = [
        {hero: "Invoker", value: 58},
        {hero: "PA", value: 62},
        {hero: "Pudge", value: 47},
    ];

    return (
        <View className="bg-white rounded-2xl p-5 shadow-md h-80 border border-black">
            <Text className="text-black text-xl font-bold mb-4">
                Hero Winrates
            </Text>
            <CartesianChart
                data={data}
                xKey="hero"
                yKeys={["value"]}
                domainPadding={{left: 20, right: 20, top: 10, bottom: 20}}
            >
                {({points, chartBounds}) => (
                    <Bar chartBounds={chartBounds} points={points.value} roundedCorners={{topLeft: 6, topRight: 6}}>
                        <LinearGradient
                            start={vec(0, 0)}
                            end={vec(0, 200)}
                            colors={["#22c55e", "#16a34a"]} // green shades
                        />
                    </Bar>
                )}
            </CartesianChart>
            <Text className="text-gray-500 text-sm mt-3 text-center">
                Last 20 matches
            </Text>
        </View>
    );
};

export default HeroWinrateWidget;
