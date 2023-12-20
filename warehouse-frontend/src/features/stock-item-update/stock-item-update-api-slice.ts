import apiSplice from '../../app/api-slice';
import ApiTagTypes from '../../types/api-tag-types';
import IUpdateStockItem from '../../types/update-stock-item';

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
