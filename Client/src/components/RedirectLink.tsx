import React from "react";
import {View, Text} from "react-native";
import {Link} from "expo-router";
import {Href} from "expo-router/build/types";

type RedirectLinkProps = {
    text: string;
    linkText: string;
    href: Href;
    align?: "left" | "center" | "right";
};

export default function RedirectLink(
    {
        text,
        linkText,
        href,
        align = "center",
    }: RedirectLinkProps) {
    return (
        <View
            className={`flex-row justify-${align}`}
        >
            {text && <Text className="text-gray-600">{text} </Text>}
            <Link href={href} asChild>
                <Text className="text-base text-black underline font-inter">
                    {linkText}
                </Text>
            </Link>
        </View>
    );
}
