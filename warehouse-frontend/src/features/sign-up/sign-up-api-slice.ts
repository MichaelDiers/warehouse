import apiSplice from '../../app/api-slice';

export interface ISignUpRequest {
  displayName: string;
  invitationCode: string;
  password: string;
  id: string;
}

/**
 * Add the sign up endpoint to the api.
 */
const signUpApiSlice = apiSplice.injectEndpoints({
  endpoints: builder => ({
    signUp: builder.mutation({
      query: (request: ISignUpRequest) => ({
        url: 'http://localhost:5008/api/Auth/sign-up',
        method: 'POST',
        body: request,
      })
    }),
  }),
  overrideExisting: false,
});

export const {
  useSignUpMutation,
} = signUpApiSlice;
