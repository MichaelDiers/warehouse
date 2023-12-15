import apiSplice from '../../app/api-slice';

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
				url: 'http://localhost:5008/api/Auth/sign-in',
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
