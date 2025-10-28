// src/lib/relayEnvironment.ts
import {
    Store,
    RecordSource,
    Environment,
    Network,
    Observable,
} from "relay-runtime";
import type {
    FetchFunction,
    IEnvironment,
    RequestParameters,
    Variables,
    CacheConfig,
    UploadableMap,
} from "relay-runtime";

import {fetchGraphQL} from "../authFetch";

const fetchFn: FetchFunction = async (
    params: RequestParameters,
    variables: Variables,
    _cacheConfig: CacheConfig,
    _uploadables?: UploadableMap | null
) => {
    if (!params.text) {
        throw new Error("Relay expected full query text but got persisted query");
    }
    return fetchGraphQL(params.text, variables);
};

export function createEnvironment(): IEnvironment {
    const network = Network.create((params, vars, cacheConfig, uploadables) =>
        Observable.from(fetchFn(params, vars, cacheConfig, uploadables))
    );
    const store = new Store(new RecordSource());
    return new Environment({store, network});
}
