import React, {useState, useEffect, createContext, useContext} from "react";
import {View, Text, TextInput, TextInputProps} from "react-native";
import Animated, {
    useSharedValue,
    useAnimatedStyle,
    withTiming,
    interpolateColor,
} from "react-native-reanimated";
import {Ionicons} from "@expo/vector-icons";
import cn from "clsx";
import ErrorTooltip from "@/src/components/ErrorTooltip";

const AnimatedView = Animated.createAnimatedComponent(View);
const FormFieldContext = createContext<FormFieldContextType>({hasError: false});

type FormFieldContextType = { error?: string; hasError: boolean };
type FormFieldInputProps = TextInputProps & {
    secure?: boolean;
    icon?: keyof typeof Ionicons.glyphMap;
};

function FormFieldRoot(
    {
        children,
        error,
    }: {
        children: React.ReactNode;
        error?: string;
    }) {
    return (
        <FormFieldContext.Provider value={{error, hasError: !!error}}>
            <View className="w-full gap-1">{children}</View>
        </FormFieldContext.Provider>
    );
}

function FormFieldLabel(
    {
        children,
    }:
    {
        children: React.ReactNode;
    }) {
    return (
        <View className="flex flex-col gap-0.5">
            <Text className="text-base font-inter-semibold text-gray-900">
                {children}
            </Text>
        </View>
    );
}


function FormFieldInput({secure, icon, ...props}: FormFieldInputProps) {
    const [isFocused, setIsFocused] = useState(false);
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);
    const {hasError} = useContext(FormFieldContext);

    const borderState = useSharedValue(0);

    useEffect(() => {
        if (hasError) {
            borderState.value = withTiming(2, {duration: 200});
        } else if (isFocused) {
            borderState.value = withTiming(1, {duration: 200});
        } else {
            borderState.value = withTiming(0, {duration: 200});
        }
    }, [hasError, isFocused]);

    const animatedStyle = useAnimatedStyle(() => {
        const borderColor = interpolateColor(
            borderState.value,
            [0, 1, 2],
            ["#D1D5DB", "#000000", "#EF4444"]
        );
        return {borderColor};
    });

    return (
        <AnimatedView
            style={[animatedStyle]}
            className={cn("flex-row items-center bg-white border rounded-[10px] px-4")}
        >
            <TextInput
                {...props}
                className="flex-1 text-lg py-3"
                secureTextEntry={secure && !isPasswordVisible}
                placeholderTextColor="#00000080"
                autoCapitalize={secure ? "none" : props.autoCapitalize}
                autoCorrect={secure ? false : props.autoCorrect}
                keyboardType={secure ? "default" : props.keyboardType}
                textContentType={secure ? "password" : props.textContentType}
                importantForAutofill={secure ? "no" : props.importantForAutofill}
                onFocus={() => setIsFocused(true)}
                onBlur={() => setIsFocused(false)}
            />
            {secure ? (
                <Ionicons
                    name={isPasswordVisible ? "eye" : "eye-off"}
                    size={20}
                    color="#00000080"
                    onPress={() => setIsPasswordVisible(!isPasswordVisible)}
                    style={{marginLeft: 8}}
                />
            ) : (
                icon && (
                    <Ionicons
                        name={icon}
                        size={20}
                        color="#00000080"
                        style={{marginLeft: 8}}
                    />
                )
            )}
        </AnimatedView>
    );
}

function FormFieldError({children}: { children?: string }) {
    const {error} = useContext(FormFieldContext);
    if (!error && !children) return null;
    return <ErrorTooltip error={children ?? error}/>;
}


export const FormField = Object.assign(FormFieldRoot, {
    Label: FormFieldLabel,
    Input: FormFieldInput,
    Error: FormFieldError,
});
