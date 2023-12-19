import apiSplice from '../../app/api-slice';
import ApiTagTypes from '../../types/api-tag-types';
import Urn from '../../types/urn.enum';

export interface ICreateStockItemRequest {
	name: string;
	minimumQuantity: string;
  quantity: string;
}

const stockItemCreateApiSlice = apiSplice.injectEndpoints({
	endpoints: builder => ({
		createStockItem: builder.mutation({
			query: (request: ICreateStockItemRequest) => ({
				url: Urn.STOCK_ITEM_CREATE,
				method: 'POST',
				body: request,
			}),
      invalidatesTags: [ApiTagTypes.STOCK_ITEM]
		}),
	}),
	overrideExisting: false,
});

export const {
	useCreateStockItemMutation,
} = stockItemCreateApiSlice;
