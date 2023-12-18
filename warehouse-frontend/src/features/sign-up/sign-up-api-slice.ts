import apiSplice from '../../app/api-slice';
import Urn from '../../types/urn.enum';

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
        url: Urn.AUTH_SIGN_UP,
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
