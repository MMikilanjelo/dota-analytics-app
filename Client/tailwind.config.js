/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./src/app/**/*.{js,jsx,ts,tsx}", "./src/components/**/*.{js,jsx,ts,tsx}"],
    presets: [require("nativewind/preset")],
    theme: {
        extend: {
            fontFamily: {
                inter: ['Inter-Regular', 'sans-serif'],
                "inter-semibold": ['Inter-Semibold', 'sans-serif'],
                "poppins": ['Poppins-Bold', 'sans-serif']
            },
            colors: {
                primary: {
                    100: "#0061FF0A",
                    200: "#0061FF1A",
                    300: "#0061FF",
                },
                accent: {
                    100: "#FBFBFD",
                },
                black: {
                    DEFAULT: "#000000",
                    100: "#8C8E98",
                    200: "#666876",
                    300: "#191D31",
                },
                white: {
                    DEFAULT: "#FFFFFF",
                },
                danger: "#F75555",
            },
        },
    },
    plugins: [],
}
