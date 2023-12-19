import apiSplice from '../../app/api-slice';
import ApiTagTypes from '../../types/api-tag-types';
import Urn from '../../types/urn.enum';

export interface IStockItem {
  name: string;
  quantity: number;
  minimumQuantity: number;
}

const stockItemsGetApiSlice = apiSplice.injectEndpoints({
	endpoints: builder => ({
		getStockItems: builder.query<IStockItem[], void>({
			query: () => ({
				url: Urn.STOCK_ITEM_READ_ALL,
				method: 'GET',
			}),
      providesTags: [ApiTagTypes.STOCK_ITEM]
		}),
	}),
	overrideExisting: false,
});

export const {
	useGetStockItemsQuery,
} = stockItemsGetApiSlice;
