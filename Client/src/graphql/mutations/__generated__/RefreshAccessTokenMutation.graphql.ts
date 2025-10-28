/**
 * @generated SignedSource<<93702277b215de95c248301e1a91607d>>
 * @lightSyntaxTransform
 * @nogrep
 */

/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ConcreteRequest } from 'relay-runtime';
export type RefreshAccessTokenInput = {
  refreshToken: string;
};
export type RefreshAccessTokenMutation$variables = {
  input: RefreshAccessTokenInput;
};
export type RefreshAccessTokenMutation$data = {
  readonly refreshAccessToken: {
    readonly accessToken: string | null | undefined;
    readonly errors: ReadonlyArray<{
      readonly __typename: "RefreshTokenExpiredError";
      readonly code: string;
      readonly message: string;
    } | {
      readonly __typename: "RefreshTokenNotFoundError";
      readonly code: string;
      readonly message: string;
    } | {
      // This will never be '%other', but we need some
      // value in case none of the concrete values match.
      readonly __typename: "%other";
    }> | null | undefined;
    readonly refreshToken: string | null | undefined;
  };
};
export type RefreshAccessTokenMutation = {
  response: RefreshAccessTokenMutation$data;
  variables: RefreshAccessTokenMutation$variables;
};

const node: ConcreteRequest = (function(){
var v0 = [
  {
    "defaultValue": null,
    "kind": "LocalArgument",
    "name": "input"
  }
],
v1 = [
  {
    "kind": "Variable",
    "name": "input",
    "variableName": "input"
  }
],
v2 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "accessToken",
  "storageKey": null
},
v3 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "refreshToken",
  "storageKey": null
},
v4 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "__typename",
  "storageKey": null
},
v5 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "message",
  "storageKey": null
},
v6 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "code",
  "storageKey": null
},
v7 = [
  (v4/*: any*/),
  (v5/*: any*/),
  (v6/*: any*/)
],
v8 = [
  (v5/*: any*/),
  (v6/*: any*/)
];
return {
  "fragment": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Fragment",
    "metadata": null,
    "name": "RefreshAccessTokenMutation",
    "selections": [
      {
        "alias": null,
        "args": (v1/*: any*/),
        "concreteType": "RefreshAccessTokenPayload",
        "kind": "LinkedField",
        "name": "refreshAccessToken",
        "plural": false,
        "selections": [
          (v2/*: any*/),
          (v3/*: any*/),
          {
            "alias": null,
            "args": null,
            "concreteType": null,
            "kind": "LinkedField",
            "name": "errors",
            "plural": true,
            "selections": [
              {
                "kind": "InlineFragment",
                "selections": (v7/*: any*/),
                "type": "RefreshTokenExpiredError",
                "abstractKey": null
              },
              {
                "kind": "InlineFragment",
                "selections": (v7/*: any*/),
                "type": "RefreshTokenNotFoundError",
                "abstractKey": null
              }
            ],
            "storageKey": null
          }
        ],
        "storageKey": null
      }
    ],
    "type": "Mutation",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "RefreshAccessTokenMutation",
    "selections": [
      {
        "alias": null,
        "args": (v1/*: any*/),
        "concreteType": "RefreshAccessTokenPayload",
        "kind": "LinkedField",
        "name": "refreshAccessToken",
        "plural": false,
        "selections": [
          (v2/*: any*/),
          (v3/*: any*/),
          {
            "alias": null,
            "args": null,
            "concreteType": null,
            "kind": "LinkedField",
            "name": "errors",
            "plural": true,
            "selections": [
              (v4/*: any*/),
              {
                "kind": "InlineFragment",
                "selections": (v8/*: any*/),
                "type": "RefreshTokenExpiredError",
                "abstractKey": null
              },
              {
                "kind": "InlineFragment",
                "selections": (v8/*: any*/),
                "type": "RefreshTokenNotFoundError",
                "abstractKey": null
              }
            ],
            "storageKey": null
          }
        ],
        "storageKey": null
      }
    ]
  },
  "params": {
    "cacheID": "9e2b2aa856c184bd7d0eead4e0e43efe",
    "id": null,
    "metadata": {},
    "name": "RefreshAccessTokenMutation",
    "operationKind": "mutation",
    "text": "mutation RefreshAccessTokenMutation(\n  $input: RefreshAccessTokenInput!\n) {\n  refreshAccessToken(input: $input) {\n    accessToken\n    refreshToken\n    errors {\n      __typename\n      ... on RefreshTokenExpiredError {\n        __typename\n        message\n        code\n      }\n      ... on RefreshTokenNotFoundError {\n        __typename\n        message\n        code\n      }\n    }\n  }\n}\n"
  }
};
})();

(node as any).hash = "fca04ffaa5a6bdfb01709420ec51f823";

export default node;
