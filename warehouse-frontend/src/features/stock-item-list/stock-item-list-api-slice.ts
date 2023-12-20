import apiSplice from '../../app/api-slice';
import ApiTagTypes from '../../types/api-tag-types';
import IFrontendStockItem from '../../types/frontend-stock-item';
import ILink from '../../types/link';
import IStockItem from '../../types/stock-item';
import Urn from '../../types/urn.enum';

const stockItemsGetApiSlice = apiSplice.injectEndpoints({
  endpoints: builder => ({
    getStockItems: builder.query<IFrontendStockItem[], void>({
      query: () => ({
        url: Urn.STOCK_ITEM_READ_ALL,
        method: 'GET',
      }),
      providesTags: [ApiTagTypes.STOCK_ITEM],
      transformResponse: (response: IFrontendStockItem[]) => {
        return response.map((stockItem: IFrontendStockItem) => {
          const backendStockItem = stockItem as IStockItem;
          const deleteUrl = backendStockItem.links.find((link: ILink) => link.urn === Urn.STOCK_ITEM_DELETE)?.url;
          const detailsUrl = backendStockItem.links.find((link: ILink) => link.urn === Urn.STOCK_ITEM_READ_BY_ID)?.url;
          const updateUrl = backendStockItem.links.find((link: ILink) => link.urn === Urn.STOCK_ITEM_UPDATE)?.url;
          return { ...stockItem, deleteUrl, detailsUrl, updateUrl };
        });
      }
    }),
  }),
  overrideExisting: false,
});

export const {
  useGetStockItemsQuery,
} = stockItemsGetApiSlice;
