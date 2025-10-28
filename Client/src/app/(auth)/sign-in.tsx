import React, {useState} from "react";
import {Text, View} from "react-native";
import {useMutation} from "react-relay";

import {signInSchema, type SignInFormValues} from "@/src/validation/signInSchema";
import PrimaryButton from "@/src/components/PrimaryButton";

import {LoginMutation} from "@/src/graphql/mutations/LoginMutation";
import type {LoginMutation as LoginMutationType} from "@/src/graphql/mutations/__generated__/LoginMutation.graphql";

import {useAuthStore} from "@/src/store/authStore";
import RedirectLink from "@/src/components/RedirectLink";
import {FormField} from "@/src/components/FormField";
import {useRouter} from "expo-router";

const SignIn = () => {
    const router = useRouter();
    const setAuth = useAuthStore((state) => state.setAuth);
    const [loginFormValues, setLoginFormValues] = useState<SignInFormValues>({email: "", password: "",});
    const [fieldErrors, setFieldErrors] = useState<Partial<SignInFormValues>>({});
    const [commitSignIn, isSigningIn] = useMutation<LoginMutationType>(LoginMutation);

    const handleSignIn = () => {
        setFieldErrors({});

        const result = signInSchema.safeParse(loginFormValues);

        if (!result.success) {

            const formatted = result.error.format();

            setFieldErrors({
                email: formatted.email?._errors[0],
                password: formatted.password?._errors[0]
            });

            return;
        }

        commitSignIn({
            variables: {input: {email: loginFormValues.email.trim(), password: loginFormValues.password}},
            onCompleted: (response) => {
                const {user, accessToken, refreshToken, errors: apiErrors} = response.loginUser;

                if (apiErrors && apiErrors.length > 0) {

                    const newFieldErrors = apiErrors.reduce<Partial<SignInFormValues>>((acc, err) => {
                        
                        if (err.__typename === "%other") return acc;

                        if (err.__typename === "InvalidCredentialsError") {
                            acc.email = "Email or password invalid";
                        }

                        if (err.__typename === "UserNotFoundError") {
                            acc.email = "No account found with this email. Please Sign up";
                        }

                        return acc;
                    }, {});

                    if (Object.keys(newFieldErrors).length > 0) {
                        setFieldErrors(newFieldErrors);
                        return;
                    }
                }

                if (accessToken && refreshToken && user) {
                    setAuth({id: user.id, email: loginFormValues.email.trim()}, accessToken, refreshToken);

                    setLoginFormValues({email: "", password: ""});

                    router.replace("/(tabs)");
                }
            },
        });
    };

    return (
        <View className="flex-1 px-4 gap-6 pt-[145px]">
            <View className="flex justify-center items-start">
                <Text className="text-3xl leading-[40px] font-poppins">Sign In</Text>
            </View>
            <View className="flex-1 gap-4">
                <FormField error={fieldErrors.email}>
                    <FormField.Label>Email</FormField.Label>
                    <FormField.Input
                        textContentType="emailAddress"
                        placeholder="Enter your email"
                        keyboardType="email-address"
                        value={loginFormValues.email}
                        onChangeText={(text) => setLoginFormValues((prev) => ({...prev, email: text}))}
                        icon="mail-outline"
                    />
                    <FormField.Error/>
                </FormField>
                <FormField error={fieldErrors.password}>
                    <FormField.Label>Password</FormField.Label>
                    <FormField.Input
                        textContentType="password"
                        placeholder="Enter your password"
                        secure
                        value={loginFormValues.password}
                        onChangeText={(text) => setLoginFormValues((prev) => ({...prev, password: text}))}
                    />
                    <FormField.Error/>
                </FormField>
                <PrimaryButton
                    title="Sign In"
                    isLoading={isSigningIn}
                    onPress={handleSignIn}
                />
                <RedirectLink text="Don’t have an account?" linkText="Sign Up" href="/sign-up"/>
            </View>
        </View>
    );
};

export default SignIn;
