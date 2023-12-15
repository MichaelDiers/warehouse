import apiSplice from '../../app/api-slice';

/**
 * Add the options endpoint to the api.
 */
const optionsApiSlice = apiSplice.injectEndpoints({
  endpoints: builder => ({
    getOptions: builder.query({
      query: () => ({
        url: 'https://localhost:7107/api/Options',
        method: 'GET'
      }),
    }),
  }),
  overrideExisting: false,
});

export const {
  useGetOptionsQuery,
} = optionsApiSlice;
