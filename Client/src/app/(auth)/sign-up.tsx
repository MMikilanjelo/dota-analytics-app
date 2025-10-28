import React, {useState} from "react";
import {View, Text} from "react-native";
import {useRouter} from "expo-router";

import PrimaryButton from "@/src/components/PrimaryButton";
import RedirectLink from "@/src/components/RedirectLink";

import {RegisterMutation} from "@/src/graphql/mutations/RegisterMutation";
import type {RegisterMutation as RegisterMutationType} from "@/src/graphql/mutations/__generated__/RegisterMutation.graphql";
import {useMutation} from "react-relay";
import {signUpSchema, type SignUpFormValues} from "@/src/validation/registerSchema";
import type {LoginMutation as LoginMutationType} from "@/src/graphql/mutations/__generated__/LoginMutation.graphql";
import {LoginMutation} from "@/src/graphql/mutations/LoginMutation";
import type {SignInFormValues} from "@/src/validation/signInSchema";
import {useAuthStore} from "@/src/store/authStore";
import {FormField} from "@/src/components/FormField";

const SignUp = () => {

    const setAuth = useAuthStore((state) => state.setAuth);

    const router = useRouter();

    const [values, setValues] = useState<SignUpFormValues>({
        email: "",
        password: "",
        confirmPassword: "",
    });

    const [fieldErrors, setFieldErrors] = useState<Partial<SignUpFormValues>>({});

    const [commitSignUp, isSigningUp] = useMutation<RegisterMutationType>(RegisterMutation);
    const [commitSignIn, isSigningIn] = useMutation<LoginMutationType>(LoginMutation);

    const handleSignUp = () => {

        setFieldErrors({});

        const result = signUpSchema.safeParse(values);

        if (!result.success) {

            const formatted = result.error.format();

            setFieldErrors({
                email: formatted.email?._errors[0],
                password: formatted.password?._errors[0],
                confirmPassword: formatted.confirmPassword?._errors[0],
            });

            return;
        }

        commitSignUp({
            variables: {input: {email: values.email.trim(), password: values.password}},
            onCompleted: (response) => {
                const {user, errors} = response.createUser;

                if (errors && errors.length > 0) {
                    const emailError = errors.find((e) => e.__typename === "NotUniqueEmailError");
                    if (emailError) {
                        setFieldErrors((prev) => ({...prev, email: emailError.message}));
                    }
                    return;
                }

                if (user) {
                    commitSignIn({
                        variables: {input: {email: values.email.trim(), password: values.password.trim()}},
                        onCompleted: (response) => {

                            const {accessToken, refreshToken, errors: apiErrors} = response.loginUser;

                            if (apiErrors && apiErrors.length > 0) {
                                const newFieldErrors: Partial<SignInFormValues> = {};

                                if (Object.keys(newFieldErrors).length > 0) {
                                    setFieldErrors(newFieldErrors);
                                    return;
                                }
                            }

                            if (accessToken && refreshToken) {
                                setAuth(
                                    {
                                        id: user.id,
                                        email: values.email.trim()
                                    },
                                    accessToken,
                                    refreshToken
                                );

                                setValues({email: "", confirmPassword: "", password: ""})

                                router.replace("/(tabs)");
                            }
                        },
                    });
                }
            },
            onError: (error) => {
                console.log(error);
            },
        });
    };

    return (
        <View className="flex-1 px-4 gap-6 pt-[145px]">
            <View className="flex justify-center items-start">
                <Text className="text-3xl leading-[40px] font-poppins">Sign Up</Text>
            </View>
            <View className="flex-1 gap-6">
                <View className="flex gap-3">
                    <FormField error={fieldErrors.email}>
                        <FormField.Label>Email</FormField.Label>
                        <FormField.Input
                            placeholder="Enter your email"
                            value={values.email}
                            textContentType="emailAddress"
                            keyboardType="email-address"
                            onChangeText={(text) => setValues((prev) => ({...prev, email: text.trim()}))}
                            icon="mail-outline"
                        />
                        <FormField.Error/>
                    </FormField>
                    <FormField error={fieldErrors.password}>
                        <FormField.Label>Password</FormField.Label>
                        <FormField.Input
                            placeholder="Enter your password"
                            secure
                            textContentType="password"
                            value={values.password}
                            onChangeText={(text) => setValues((prev) => ({...prev, password: text.trim()}))}
                        />
                        <FormField.Error/>
                    </FormField>

                    <FormField error={fieldErrors.confirmPassword}>
                        <FormField.Label>Confirm Password</FormField.Label>
                        <FormField.Input
                            placeholder="Enter your password again"
                            secure
                            textContentType="password"
                            value={values.confirmPassword}
                            onChangeText={(text) =>
                                setValues((prev) => ({...prev, confirmPassword: text.trim()}))
                            }
                        />
                        <FormField.Error/>
                    </FormField>
                </View>
                <PrimaryButton
                    title="Create Account"
                    onPress={handleSignUp}
                    isLoading={isSigningUp && isSigningIn}
                />
                <RedirectLink text="Already have an account?" linkText="Sign In" href="/sign-in"/>
            </View>
        </View>
    );
};

export default SignUp;
