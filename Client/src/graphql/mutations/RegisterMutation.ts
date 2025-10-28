import {graphql} from "react-relay";

export const RegisterMutation = graphql`
    mutation RegisterMutation($input: CreateUserInput!) {
        createUser(input: $input) {
            user {
                id
                email
            }
            errors {
                ... on NotUniqueEmailError {
                    __typename
                    code
                    message
                }
                ... on ValidationError {
                    __typename
                    code
                    message
                    errors {
                        __typename
                        message
                        code
                    }
                }
            }
        }
    }
`;