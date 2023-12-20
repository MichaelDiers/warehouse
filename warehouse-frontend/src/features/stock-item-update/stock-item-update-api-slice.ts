import apiSplice from '../../app/api-slice';
import ApiTagTypes from '../../types/api-tag-types';
import ICreateStockItem from '../../types/create-stock-item';
import IUpdateStockItem from '../../types/update-stock-item';
import Urn from '../../types/urn.enum';

const stockItemUpdateApiSlice = apiSplice.injectEndpoints({
  endpoints: builder => ({
    updateStockItem: builder.mutation({
      query: ({ request, url } : { request: IUpdateStockItem, url: string }) => ({
        url,
        method: 'PUT',
        body: request,
      }),
      invalidatesTags: [ApiTagTypes.STOCK_ITEM]
    }),
  }),
  overrideExisting: false,
});

export const {
  useUpdateStockItemMutation,
} = stockItemUpdateApiSlice;
