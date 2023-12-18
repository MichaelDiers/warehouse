import apiSplice from '../../app/api-slice';
import Urn from '../../types/urn.enum';

export interface ISignInRequest {
	id: string;
	password: string;

}

/**
 * Add the sign up endpoint to the api.
 */
const signInApiSlice = apiSplice.injectEndpoints({
	endpoints: builder => ({
		signIn: builder.mutation({
			query: (request: ISignInRequest) => ({
				url: Urn.AUTH_SIGN_IN,
				method: 'POST',
				body: request,
			})
		}),
	}),
	overrideExisting: false,
});

export const {
	useSignInMutation,
} = signInApiSlice;
