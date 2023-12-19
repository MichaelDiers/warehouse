import apiSplice from '../../app/api-slice';
import ApiTagTypes from '../../types/api-tag-types';
import ICreateStockItem from '../../types/create-stock-item';
import Urn from '../../types/urn.enum';

const stockItemCreateApiSlice = apiSplice.injectEndpoints({
  endpoints: builder => ({
    createStockItem: builder.mutation({
      query: (request: ICreateStockItem) => ({
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
