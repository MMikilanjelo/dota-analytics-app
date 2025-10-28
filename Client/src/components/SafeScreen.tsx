import React, {ReactNode} from "react";
import {View, StyleProp, ViewStyle} from "react-native";
import {useSafeAreaInsets} from "react-native-safe-area-context";
import cn from "clsx";

interface SafeScreenProps {
    children: ReactNode;
    className?: string;
    style?: StyleProp<ViewStyle>;
}

const SafeScreen: React.FC<SafeScreenProps> = ({children, className = "", style}) => {
    const insets = useSafeAreaInsets();

    return (
        <View
            className={cn("flex-1 bg-white", className)}
            style={[
                {
                    paddingTop: insets.top,
                    paddingBottom: insets.bottom,
                    paddingLeft: insets.left,
                    paddingRight: insets.right,
                },
                style,
            ]}
        >
            {children}
        </View>
    );
};

export default SafeScreen;
