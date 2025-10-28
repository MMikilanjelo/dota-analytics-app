import {graphql} from 'react-relay';

export const LoginMutation = graphql`
    mutation LoginMutation($input: LoginUserInput!) {
        loginUser(input: $input) {
            accessToken
            refreshToken
            user {
                id
                email
            }
            errors {
                ... on InvalidCredentialsError {
                    __typename
                    message
                    code
                }
                ... on ValidationError {
                    __typename
                    message
                    errors {
                        __typename
                        code
                        message
                    }
                    code
                }
                ... on UserNotFoundError{
                    __typename
                    message
                    code
                }
            }
        }
    }
`;

