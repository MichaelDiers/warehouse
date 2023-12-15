import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

/**
 * The base api slice for service calls. Each feature injects its endpoints
 * and enhance the api slice by new tags.
 */
export const apiSlice = createApi({
  baseQuery: fetchBaseQuery({
    baseUrl: '/',
    prepareHeaders: (headers, {getState}) => {
        headers.set('Content-Type', 'application/json');
        headers.set('x-api-key', 'The api key');
        return headers;
    }
  }),
  endpoints: () => ({}),
});

export default apiSlice;
