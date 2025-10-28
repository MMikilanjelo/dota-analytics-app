/**
 * @generated SignedSource<<8037fa727e9ac938e1c398fb2e664caf>>
 * @lightSyntaxTransform
 * @nogrep
 */

/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ConcreteRequest } from 'relay-runtime';
export type LoginUserInput = {
  email: string;
  password: string;
};
export type LoginMutation$variables = {
  input: LoginUserInput;
};
export type LoginMutation$data = {
  readonly loginUser: {
    readonly accessToken: string | null | undefined;
    readonly errors: ReadonlyArray<{
      readonly __typename: "InvalidCredentialsError";
      readonly code: string;
      readonly message: string;
    } | {
      readonly __typename: "UserNotFoundError";
      readonly code: string;
      readonly message: string;
    } | {
      readonly __typename: "ValidationError";
      readonly code: string;
      readonly errors: ReadonlyArray<{
        readonly __typename: "DomainError";
        readonly code: string;
        readonly message: string;
      }>;
      readonly message: string;
    } | {
      // This will never be '%other', but we need some
      // value in case none of the concrete values match.
      readonly __typename: "%other";
    }> | null | undefined;
    readonly refreshToken: string | null | undefined;
    readonly user: {
      readonly email: string;
      readonly id: string;
    } | null | undefined;
  };
};
export type LoginMutation = {
  response: LoginMutation$data;
  variables: LoginMutation$variables;
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
  "concreteType": "User",
  "kind": "LinkedField",
  "name": "user",
  "plural": false,
  "selections": [
    {
      "alias": null,
      "args": null,
      "kind": "ScalarField",
      "name": "id",
      "storageKey": null
    },
    {
      "alias": null,
      "args": null,
      "kind": "ScalarField",
      "name": "email",
      "storageKey": null
    }
  ],
  "storageKey": null
},
v5 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "__typename",
  "storageKey": null
},
v6 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "message",
  "storageKey": null
},
v7 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "code",
  "storageKey": null
},
v8 = [
  (v5/*: any*/),
  (v6/*: any*/),
  (v7/*: any*/)
],
v9 = {
  "alias": null,
  "args": null,
  "concreteType": "DomainError",
  "kind": "LinkedField",
  "name": "errors",
  "plural": true,
  "selections": [
    (v5/*: any*/),
    (v7/*: any*/),
    (v6/*: any*/)
  ],
  "storageKey": null
},
v10 = [
  (v6/*: any*/),
  (v7/*: any*/)
];
return {
  "fragment": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Fragment",
    "metadata": null,
    "name": "LoginMutation",
    "selections": [
      {
        "alias": null,
        "args": (v1/*: any*/),
        "concreteType": "LoginUserPayload",
        "kind": "LinkedField",
        "name": "loginUser",
        "plural": false,
        "selections": [
          (v2/*: any*/),
          (v3/*: any*/),
          (v4/*: any*/),
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
                "selections": (v8/*: any*/),
                "type": "InvalidCredentialsError",
                "abstractKey": null
              },
              {
                "kind": "InlineFragment",
                "selections": [
                  (v5/*: any*/),
                  (v6/*: any*/),
                  (v9/*: any*/),
                  (v7/*: any*/)
                ],
                "type": "ValidationError",
                "abstractKey": null
              },
              {
                "kind": "InlineFragment",
                "selections": (v8/*: any*/),
                "type": "UserNotFoundError",
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
    "name": "LoginMutation",
    "selections": [
      {
        "alias": null,
        "args": (v1/*: any*/),
        "concreteType": "LoginUserPayload",
        "kind": "LinkedField",
        "name": "loginUser",
        "plural": false,
        "selections": [
          (v2/*: any*/),
          (v3/*: any*/),
          (v4/*: any*/),
          {
            "alias": null,
            "args": null,
            "concreteType": null,
            "kind": "LinkedField",
            "name": "errors",
            "plural": true,
            "selections": [
              (v5/*: any*/),
              {
                "kind": "InlineFragment",
                "selections": (v10/*: any*/),
                "type": "InvalidCredentialsError",
                "abstractKey": null
              },
              {
                "kind": "InlineFragment",
                "selections": [
                  (v6/*: any*/),
                  (v9/*: any*/),
                  (v7/*: any*/)
                ],
                "type": "ValidationError",
                "abstractKey": null
              },
              {
                "kind": "InlineFragment",
                "selections": (v10/*: any*/),
                "type": "UserNotFoundError",
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
    "cacheID": "8a2c82f14ce0277068a4233b2968c5d8",
    "id": null,
    "metadata": {},
    "name": "LoginMutation",
    "operationKind": "mutation",
    "text": "mutation LoginMutation(\n  $input: LoginUserInput!\n) {\n  loginUser(input: $input) {\n    accessToken\n    refreshToken\n    user {\n      id\n      email\n    }\n    errors {\n      __typename\n      ... on InvalidCredentialsError {\n        __typename\n        message\n        code\n      }\n      ... on ValidationError {\n        __typename\n        message\n        errors {\n          __typename\n          code\n          message\n        }\n        code\n      }\n      ... on UserNotFoundError {\n        __typename\n        message\n        code\n      }\n    }\n  }\n}\n"
  }
};
})();

(node as any).hash = "23d43c8143272f8556e6a48ca355141a";

export default node;
