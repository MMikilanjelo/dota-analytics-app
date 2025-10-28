import {z} from "zod";

export const signUpSchema = z
    .object({
        email: z.string().email("Invalid email"),
        password: z.string().min(6, "Password must be at least 6 characters"),
        confirmPassword: z.string().min(6, "Confirm password must be at least 6 characters"),
    })
    .refine((data) => data.password === data.confirmPassword, {
        path: ["confirmPassword"],
        message: "Passwords do not match",
    });


export type SignUpFormValues = z.infer<typeof signUpSchema>;
