import {graphql} from 'react-relay';

export const RefreshAccessTokenMutation = graphql`
    mutation RefreshAccessTokenMutation($input: RefreshAccessTokenInput!) {
        refreshAccessToken(input: $input) {
            accessToken
            refreshToken
            errors {
                ... on RefreshTokenExpiredError{
                    __typename
                    message
                    code
                }
                ... on RefreshTokenNotFoundError{
                    __typename
                    message
                    code
                }
            }
        }
    }
`;

