import React from "react";
import {View} from "react-native";
import TabBarButton from "./TabBarButton";
import {BottomTabBarProps} from "@react-navigation/bottom-tabs";
import {Home, User, BarChart3} from "lucide-react-native";

const TabBar: React.FC<BottomTabBarProps> = ({state, descriptors, navigation}) => {
    return (
        <View
            className="
        absolute bottom-6
        flex-row justify-between items-center
        bg-white
        mx-5 py-4
        rounded-3xl
        shadow-lg
      "
        >
            {state.routes.map((route, index) => {
                const {options} = descriptors[route.key];
                const label =
                    options.tabBarLabel !== undefined
                        ? (options.tabBarLabel as string)
                        : options.title !== undefined
                            ? options.title
                            : route.name;

                if (["_sitemap", "+not-found"].includes(route.name)) return null;

                const isFocused = state.index === index;

                const onPress = () => {
                    const event = navigation.emit({
                        type: "tabPress",
                        target: route.key,
                        canPreventDefault: true,
                    });

                    if (!isFocused && !event.defaultPrevented) {
                        navigation.navigate(route.name, route.params);
                    }
                };

                const onLongPress = () => {
                    navigation.emit({
                        type: "tabLongPress",
                        target: route.key,
                    });
                };

                let IconComponent;
                let colorClass;

                switch (route.name) {
                    case "index":
                        IconComponent = <Home color={isFocused ? "#0891b2" : "#737373"} size={24}/>;
                        colorClass = isFocused ? "text-cyan-600" : "text-gray-500";
                        break;
                    case "analytics":
                        IconComponent = <BarChart3 color={isFocused ? "#0891b2" : "#737373"} size={24}/>;
                        colorClass = isFocused ? "text-cyan-600" : "text-gray-500";
                        break;
                    case "profile":
                        IconComponent = <User color={isFocused ? "#0891b2" : "#737373"} size={24}/>;
                        colorClass = isFocused ? "text-cyan-600" : "text-gray-500";
                        break;
                    default:
                        IconComponent = null;
                        colorClass = "text-gray-500";
                }

                return (
                    <TabBarButton
                        key={route.key}
                        onPress={onPress}
                        onLongPress={onLongPress}
                        isFocused={isFocused}
                        colorClass={colorClass}   
                        label={label?.toString()}
                        icon={IconComponent}
                    />
                );
            })}
        </View>
    );
};

export default TabBar;
